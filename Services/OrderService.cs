using AutoMapper;
using DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository,IMapper mapper, ICartService cartService, ILogger<OrderService> logger, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cartService = cartService;
            _logger = logger;
            _productRepository = productRepository;
        }
        private async Task<double> CalculateRealSumAsync(List<CartItemDTO> items, double basicSitePrice)
        {
            double total = basicSitePrice;
            foreach (var item in items)
            {
                var product = await _productRepository.GetProductByIdAsync((int)item.ProductId);
                if (product == null)
                {
                    _logger.LogError("Security Warning: Product ID {ProductId} not found during validation.", item.ProductId);
                    throw new KeyNotFoundException("Product not found");
                }
                total += product.Price;
            }
            return total;
        }

        private bool IsCartTotalConsistent(CartDTO cartDto, double expectedSum)
        {
            return Math.Abs(cartDto.TotalPrice - expectedSum) < 0.01;
        }

        public async Task<(IEnumerable<OrderDetailsDTO> Orders, double Total)> GetOrdersAsync()
        {
            double total = 0;
            var orders = await _orderRepository.GetOrdersAsync();
            foreach (var order in orders)
            {
                total += order.OrderSum;
            }
            var orderDtos = _mapper.Map<IEnumerable<OrderDetailsDTO>>(orders);
            return (orderDtos, total);
        }

        public async Task<OrderDetailsDTO> AddOrderFromCartAsync(int cartId)
        {
            ValidateCartId(cartId);
            var cartDto = await GetValidatedCartAsync(cartId);
            var expectedSum = await CalculateRealSumAsync(cartDto.CartItems, cartDto.BasicSitePrice);
            ValidateCartTotalConsistency(cartId, cartDto, expectedSum);
            var order = BuildOrderFromCart(cartDto, expectedSum);

            return await SaveOrderAndClearCartAsync(order, cartId);
        }

        private static void ValidateCartId(int cartId)
        {
            if (cartId <= 0)
            {
                throw new InvalidOperationException("Invalid cart ID.");
            }
        }

        private async Task<CartDTO> GetValidatedCartAsync(int cartId)
        {
            var cartDto = await _cartService.GetCartByIdAsync(cartId);
            if (cartDto == null)
            {
                throw new KeyNotFoundException("Cart not found.");
            }

            if (cartDto.CartItems == null || cartDto.CartItems.Count == 0)
            {
                throw new InvalidOperationException("The cart is empty.");
            }

            if (!cartDto.BasicSiteId.HasValue)
            {
                throw new InvalidOperationException("Cannot create order without BasicSiteId.");
            }

            return cartDto;
        }

        private void ValidateCartTotalConsistency(int cartId, CartDTO cartDto, double expectedSum)
        {
            if (!IsCartTotalConsistent(cartDto, expectedSum))
            {
                _logger.LogWarning("SECURITY ALERT: Cart total mismatch for Cart {CartId}. Cart.TotalPrice={CartTotalPrice}, Expected={ExpectedSum}",
                    cartId, cartDto.TotalPrice, expectedSum);
                throw new InvalidOperationException("Cart total validation failed.");
            }
        }

        private static Order BuildOrderFromCart(CartDTO cartDto, double expectedSum)
        {
            return new Order
            {
                UserId = cartDto.UserId,
                BasicSiteId = cartDto.BasicSiteId!.Value,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = expectedSum,
                Status = 1,
                OrderItems = cartDto.CartItems.Select(item => new OrderItem
                {
                    PromptId = item.PromptId,
                    PlatformId = item.PlatformId ?? 1,
                    ProductId = item.ProductId
                }).ToList()
            };
        }

        private async Task<OrderDetailsDTO> SaveOrderAndClearCartAsync(Order order, int cartId)
        {
            var createdOrder = await _orderRepository.AddOrderAsync(order);
            await _cartService.ClearCartAsync(cartId);
            return _mapper.Map<OrderDetailsDTO>(createdOrder);
        }

        public async Task<OrderDetailsDTO?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }
            return _mapper.Map<OrderDetailsDTO>(order);
        }
       
        public async Task UpdateStatusAsync(OrderSummaryDTO dto)
        {
            var order = _mapper.Map<Order>(dto);
            await _orderRepository.UpdateStatusAsync(order);
        }
        public async Task<ReviewDTO?> AddReviewAsync(int orderId, AddReviewDTO dto)
        {
            var existingReview = await _orderRepository.GetReviewByOrderIdAsync(orderId);
            if (existingReview != null)
            {
                return null;
            }
            var reviewDto = dto with { OrderId = orderId };
            var review = _mapper.Map<Review>(reviewDto);
            review = await _orderRepository.AddReviewAsync(review);
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO?> GetReviewByOrderIdAsync(int orderId)
        {
            var review = await _orderRepository.GetReviewByOrderIdAsync(orderId);
            if (review == null)
            {
                return null;
            }
            return _mapper.Map<ReviewDTO>(review);
        }
        public async Task UpdateReviewAsync(ReviewDTO dto)
        {
            var review = _mapper.Map<Review>(dto);
            await _orderRepository.UpdateReviewAsync(review);
        }
        public async Task<IEnumerable<OrderItemDTO>?> GetOrderItemsAsync(int orderId)
        {
            var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);

            if (orderItems == null)
                return null;

            return _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
        }
    }
}

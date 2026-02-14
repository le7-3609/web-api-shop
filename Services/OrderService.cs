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
        private async Task<float> CalculateRealSumAsync(List<CartItemDTO> items)
        {
            float total = 0;
            foreach (var item in items)
            {
                var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogError("Security Warning: Product ID {ProductId} not found during validation.", item.ProductId);
                    throw new KeyNotFoundException("Product not found");
                }
                total += (float)product.Price;
            }
            return total;
        }

        private async Task<(bool IsValid, float CalculatedSum)> ValidateOrderSumAsync(CartDTO cartDto)
        {
            float calculatedSum = await CalculateRealSumAsync(cartDto.CartItems);

            if (Math.Abs(calculatedSum - cartDto.TotalPrice) > 0)
            {
                _logger.LogError("Security Warning: Order sum mismatch. Client: {ClientSum}, Real: {RealSum}",
                    cartDto.TotalPrice, calculatedSum);
                return (false, calculatedSum);
            }
            return (true, calculatedSum);
        }

        public async Task<OrderDetailsDTO> AddOrderFromCartAsync(CartDTO cartDto)
        {
            var (isValid, expectedSum) = await ValidateOrderSumAsync(cartDto);

            if (!isValid)
            {
                _logger.LogWarning("SECURITY ALERT: Price mismatch for User {UserId}. Client sent {ClientSum}, but expected {ExpectedSum}.",
                    cartDto.UserId, cartDto.TotalPrice, expectedSum);
                throw new InvalidOperationException("Payment verification failed. The transaction has been logged.");
            }

            var order = new Order
            {
                UserId = cartDto.UserId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = expectedSum,
                Status = 1,
                OrderItems = cartDto.CartItems.Select(item => new OrderItem
                {
                    UserDescription = item.UserDescription,
                    PlatformId = item.PlatformId,
                    ProductId = item.ProductId
                }).ToList()
            };

            var createdOrder = await _orderRepository.AddOrderAsync(order);
            await _cartService.ClearCartAsync(cartDto.CartId);
            return _mapper.Map<OrderDetailsDTO>(createdOrder);
        }

        public async Task<OrderDetailsDTO> GetByIdAsync(int id)
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
        public async Task<ReviewDTO> AddReviewAsync(int orderId, AddReviewDTO dto)
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

        public async Task<ReviewDTO> GetReviewByOrderIdAsync(int orderId)
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
        public async Task<IEnumerable<OrderItemDTO>> GetOrderItemsAsync(int orderId)
        {
            var orderItems = await _orderRepository.GetOrderItemsAsync(orderId);

            if (orderItems == null)
                return null;

            return _mapper.Map<IEnumerable<OrderItemDTO>>(orderItems);
        }
    }
}

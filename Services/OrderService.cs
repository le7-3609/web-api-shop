using AutoMapper;
using DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;


        public OrderService(IOrderRepository orderRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;   
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
        public async Task<OrderDetailsDTO> AddOrderAsync(OrderSummaryDTO dto)
        {
            var order = _mapper.Map<Order>(dto);
            order = await _orderRepository.AddOrderAsync(order);
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

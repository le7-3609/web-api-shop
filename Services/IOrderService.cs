using DTO;
using Entities;

namespace Services
{
    public interface IOrderService
    {
        Task<OrderDetailsDTO> GetByIdAsync(int id);
        Task<OrderDetailsDTO> AddOrderFromCartAsync(CartDTO cartDto);
        Task UpdateStatusAsync(OrderSummaryDTO dto);
        Task<ReviewDTO> AddReviewAsync(int orderId, AddReviewDTO dto);
        Task<ReviewDTO> GetReviewByOrderIdAsync(int orderId);
        Task UpdateReviewAsync(ReviewDTO dto);
        Task<IEnumerable<OrderItemDTO>> GetOrderItemsAsync(int orderId);

    }
}
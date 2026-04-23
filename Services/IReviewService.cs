using DTO;

namespace Services
{
    public interface IReviewService
    {
        Task<ReviewDTO?> AddReviewAsync(int orderId, AddReviewDTO dto);
        Task<ReviewDTO?> GetReviewByOrderIdAsync(int orderId);
        Task UpdateReviewAsync(ReviewDTO dto);
        Task<IEnumerable<ReviewSummaryDTO>> GetAllReviewsAsync();
    }
}

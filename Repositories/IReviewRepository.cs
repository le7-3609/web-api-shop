using Entities;

namespace Repositories
{
    public interface IReviewRepository
    {
        Task<Review> AddReviewAsync(Review review);
        Task<Review?> GetReviewByOrderIdAsync(int orderId);
        Task<Review> UpdateReviewAsync(Review review);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
    }
}

using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly MyShopContext _context;

        public ReviewRepository(MyShopContext context)
        {
            _context = context;
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> GetReviewByOrderIdAsync(int orderId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.OrderId == orderId);
        }

        public async Task<Review> UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.Order)
                    .ThenInclude(o => o.BasicSite)
                        .ThenInclude(bs => bs.SiteType)
                .ToListAsync();
        }
    }
}

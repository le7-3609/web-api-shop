using Entities;
using Repositories;
using Xunit;

namespace Tests.IntegrationTests
{
    [Collection("Database collection")]
    public class ReviewRepositoryIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly MyShopContext _context;
        private readonly ReviewRepository _repository;

        public ReviewRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _repository = new ReviewRepository(_context);
            _fixture.ClearDatabase();
        }

        #region Happy Paths

        [Fact]
        public async Task AddReviewAsync_WithValidReview_ReturnsReview()
        {
            var order = new Order { OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var review = new Review { OrderId = order.OrderId, Score = 5, Note = "Great!", ReviewImageUrl = "url" };
            var result = await _repository.AddReviewAsync(review);

            Assert.True(result.ReviewId > 0);
        }

        [Fact]
        public async Task GetReviewByOrderIdAsync_WithExistingReview_ReturnsReview()
        {
            var order = new Order { OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var review = new Review { OrderId = order.OrderId, Score = 4, Note = "Good", ReviewImageUrl = "url" };
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            var result = await _repository.GetReviewByOrderIdAsync((int)order.OrderId);

            Assert.NotNull(result);
            Assert.Equal((short)4, result.Score);
        }

        [Fact]
        public async Task UpdateReviewAsync_WithValidReview_UpdatesReview()
        {
            var order = new Order { OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var review = new Review { OrderId = order.OrderId, Score = 3, Note = "Old", ReviewImageUrl = "url" };
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            review.Score = 5;
            review.Note = "Updated";
            var result = await _repository.UpdateReviewAsync(review);

            Assert.Equal((short)5, result.Score);
        }

        [Fact]
        public async Task GetAllReviewsAsync_WithReviews_ReturnsAll()
        {
            var order = new Order { OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            await _context.Reviews.AddAsync(new Review { OrderId = order.OrderId, Score = 5, Note = "A", ReviewImageUrl = "u" });
            await _context.Reviews.AddAsync(new Review { OrderId = order.OrderId, Score = 3, Note = "B", ReviewImageUrl = "u" });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllReviewsAsync();

            Assert.Equal(2, result.Count());
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetReviewByOrderIdAsync_NoReview_ReturnsNull()
        {
            var order = new Order { OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var result = await _repository.GetReviewByOrderIdAsync((int)order.OrderId);

            Assert.Null(result);
        }

        #endregion
    }
}

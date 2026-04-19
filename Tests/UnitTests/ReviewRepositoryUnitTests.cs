using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Xunit;

namespace Tests.UnitTests
{
    public class ReviewRepositoryUnitTests
    {
        #region Happy Paths

        [Fact]
        public async Task AddReviewAsync_ValidReview_ReturnsAddedReview()
        {
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.Reviews).ReturnsDbSet(new List<Review>());
            var repo = new ReviewRepository(mockContext.Object);
            var review = new Review { OrderId = 1, Note = "Great!", Score = 5 };

            var result = await repo.AddReviewAsync(review);

            Assert.Equal("Great!", result.Note);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetReviewByOrderIdAsync_ExistingOrder_ReturnsReview()
        {
            var reviews = new List<Review> { new Review { ReviewId = 1, OrderId = 1, Note = "Nice" } };
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.Reviews).ReturnsDbSet(reviews);
            var repo = new ReviewRepository(mockContext.Object);

            var result = await repo.GetReviewByOrderIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.OrderId);
        }

        [Fact]
        public async Task UpdateReviewAsync_ValidReview_CallsSaveChanges()
        {
            var review = new Review { ReviewId = 1, OrderId = 1, Score = 3, Note = "Old" };
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.Reviews).ReturnsDbSet(new List<Review> { review });
            var repo = new ReviewRepository(mockContext.Object);

            review.Score = 5;
            review.Note = "Updated";
            var result = await repo.UpdateReviewAsync(review);

            Assert.Equal((short)5, result.Score);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetReviewByOrderIdAsync_NoReview_ReturnsNull()
        {
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.Reviews).ReturnsDbSet(new List<Review>());
            var repo = new ReviewRepository(mockContext.Object);

            var result = await repo.GetReviewByOrderIdAsync(5);

            Assert.Null(result);
        }

        #endregion
    }
}

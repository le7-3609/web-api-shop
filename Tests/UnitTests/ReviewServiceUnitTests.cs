using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories;
using Services;
using Xunit;

namespace Tests.UnitTests
{
    public class ReviewServiceUnitTests
    {
        private readonly Mock<IReviewRepository> _mockReviewRepo = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<IHostEnvironment> _mockHostEnvironment = new();
        private readonly Mock<ILogger<ReviewService>> _mockLogger = new();

        private ReviewService CreateService() => new(
            _mockReviewRepo.Object, _mockMapper.Object,
            _mockHostEnvironment.Object, _mockLogger.Object);

        [Fact]
        public async Task AddReviewAsync_ReviewAlreadyExists_ReturnsNull()
        {
            _mockReviewRepo.Setup(r => r.GetReviewByOrderIdAsync(1))
                .ReturnsAsync(new Review { ReviewId = 1, OrderId = 1, Score = 5 });

            var svc = CreateService();
            var result = await svc.AddReviewAsync(1, new AddReviewDTO(1, 5, "Great", null));

            Assert.Null(result);
        }

        [Fact]
        public async Task AddReviewAsync_InvalidScore_Throws()
        {
            _mockReviewRepo.Setup(r => r.GetReviewByOrderIdAsync(1)).ReturnsAsync((Review?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                svc.AddReviewAsync(1, new AddReviewDTO(1, 6, "Bad score", null)));
        }

        [Fact]
        public async Task AddReviewAsync_ValidReview_ReturnsDTO()
        {
            _mockReviewRepo.Setup(r => r.GetReviewByOrderIdAsync(1)).ReturnsAsync((Review?)null);

            var review = new Review { ReviewId = 10, OrderId = 1, Score = 4, Note = "Good" };
            _mockMapper.Setup(m => m.Map<Review>(It.IsAny<AddReviewDTO>())).Returns(review);
            _mockReviewRepo.Setup(r => r.AddReviewAsync(It.IsAny<Review>())).ReturnsAsync(review);
            _mockMapper.Setup(m => m.Map<ReviewDTO>(It.IsAny<Review>()))
                .Returns(new ReviewDTO(10, 1, 4, "Good", ""));

            var svc = CreateService();
            var result = await svc.AddReviewAsync(1, new AddReviewDTO(1, 4, "Good", null));

            Assert.NotNull(result);
            Assert.Equal(10, result.ReviewId);
        }

        [Fact]
        public async Task GetReviewByOrderIdAsync_NotFound_ReturnsNull()
        {
            _mockReviewRepo.Setup(r => r.GetReviewByOrderIdAsync(99)).ReturnsAsync((Review?)null);

            var svc = CreateService();
            var result = await svc.GetReviewByOrderIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewByOrderIdAsync_Found_ReturnsDTO()
        {
            var review = new Review { ReviewId = 5, OrderId = 2, Score = 3 };
            _mockReviewRepo.Setup(r => r.GetReviewByOrderIdAsync(2)).ReturnsAsync(review);
            _mockMapper.Setup(m => m.Map<ReviewDTO>(review))
                .Returns(new ReviewDTO(5, 2, 3, "", ""));

            var svc = CreateService();
            var result = await svc.GetReviewByOrderIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal(5, result.ReviewId);
        }

        [Fact]
        public async Task UpdateReviewAsync_CallsRepository()
        {
            var dto = new ReviewDTO(7, 3, 5, "Updated", "url");
            var review = new Review { ReviewId = 7, OrderId = 3, Score = 5 };
            _mockMapper.Setup(m => m.Map<Review>(dto)).Returns(review);

            var svc = CreateService();
            await svc.UpdateReviewAsync(dto);

            _mockReviewRepo.Verify(r => r.UpdateReviewAsync(It.Is<Review>(rv => rv.ReviewId == 7)), Times.Once);
        }

        [Fact]
        public async Task GetAllReviewsAsync_ReturnsAllMapped()
        {
            var reviews = new List<Review>
            {
                new Review { ReviewId = 1, Score = 5 },
                new Review { ReviewId = 2, Score = 3 }
            };
            _mockReviewRepo.Setup(r => r.GetAllReviewsAsync()).ReturnsAsync(reviews);
            _mockMapper.Setup(m => m.Map<IEnumerable<ReviewSummaryDTO>>(reviews))
                .Returns(new List<ReviewSummaryDTO>
                {
                    new ReviewSummaryDTO { ReviewId = 1, Score = 5 },
                    new ReviewSummaryDTO { ReviewId = 2, Score = 3 }
                });

            var svc = CreateService();
            var result = await svc.GetAllReviewsAsync();

            Assert.Equal(2, result.Count());
        }
    }
}

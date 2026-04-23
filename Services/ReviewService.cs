using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories;

namespace Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, IHostEnvironment hostEnvironment, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task<ReviewDTO?> AddReviewAsync(int orderId, AddReviewDTO dto)
        {
            var existingReview = await _reviewRepository.GetReviewByOrderIdAsync(orderId);
            if (existingReview != null) return null;

            if (dto.Score < 1 || dto.Score > 5)
            {
                throw new ArgumentException("Review score must be between 1 and 5.");
            }

            string? relativePath = dto.Image != null ? await SaveImageToFileSystemAsync(dto.Image) : null;

            var review = _mapper.Map<Review>(dto);
            review.OrderId = orderId;
            review.ReviewImageUrl = relativePath;

            var savedReview = await _reviewRepository.AddReviewAsync(review);
            return _mapper.Map<ReviewDTO>(savedReview);
        }

        public async Task<ReviewDTO?> GetReviewByOrderIdAsync(int orderId)
        {
            var review = await _reviewRepository.GetReviewByOrderIdAsync(orderId);
            if (review == null) return null;
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task UpdateReviewAsync(ReviewDTO dto)
        {
            var review = _mapper.Map<Review>(dto);
            await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task<IEnumerable<ReviewSummaryDTO>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllReviewsAsync();
            return _mapper.Map<IEnumerable<ReviewSummaryDTO>>(reviews);
        }

        private async Task<string?> SaveImageToFileSystemAsync(IFormFile image)
        {
            if (image == null || image.Length == 0) return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return "/uploads/" + fileName;
        }
    }
}

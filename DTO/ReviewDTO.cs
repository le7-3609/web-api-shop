using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public record AddReviewDTO
    (
        long OrderId,
        [Range(1, 5, ErrorMessage = "Review score must be between 1 and 5.")]
        short Score,
        string? Note,
        IFormFile? Image,
        string? ReviewImageUrl = null
    );

    public record ReviewDTO
    (
        long ReviewId,
        long OrderId,
        short Score,
        string Note,
        string ReviewImageUrl
    );

    public record ReviewSummaryDTO
    {
        public long ReviewId { get; init; }
        public string? ReviewImageUrl { get; init; }
        public string? Note { get; init; }
        public short Score { get; init; }
        public string SiteName { get; init; }
        public string SiteTypeName { get; init; }
    }
}

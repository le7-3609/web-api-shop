using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record OrderItemDTO
    {
        public int OrderItemId { get; init; }
        public string ProductName { get; init; }
        public float Price { get; init; }
        public string PlatformName { get; init; }
        public string UserDescription { get; init; }
    }

    public record OrderSummaryDTO
    {
        public int OrderId { get; init; }
        public DateOnly? OrderDate { get; init; }
        public float OrderSum { get; init; }
        public string StatusName { get; init; }
    }

    public record OrderDetailsDTO : OrderSummaryDTO
    {
        public int OrderId { get; init; }
        public DateOnly? OrderDate { get; init; }
        public float OrderSum { get; init; }
        public int OrderItemsCount { get; init; }
        public string StatusName { get; init; }
        public int UserId { get; init; }
        public int ReviewId { get; init; }
        public string ReviewImageUrl { get; init; }
        public int Score { get; init; }
        public string SiteName { get; init; }
        public string SiteTypeName { get; init; }
        public string SiteDescription { get; init; }
        public List<OrderItemDTO> Items { get; init; }
    } 

    public record AddReviewDTO(
        int OrderId,
        int Score,
        string Note,
        string ReviewImageUrl
    );

    public record ReviewDTO(
        int ReviewId,
        int OrderId,
        int Score,
        string Note,
        string ReviewImageUrl
    );
}
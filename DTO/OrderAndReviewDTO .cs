using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record OrderItemDTO(
        int OrderItemId,
        string ProductName,
        float Price,
        string PlatformName,
        string UserDescription
    );

    public record CreateOrderDTO(
        int UserId,
        int OrderSum,//לשנות לdecimal 
        List<AddCartItemDTO> CartItem,
        string SiteName,
        string SiteTypeId
    );
    public record OrderSummaryDTO(
        int OrderId,
        DateTime OrderDate,
        float TotalAmount,
        string OrderStatus,
        int OrderItemsCount
    );

    public record OrderDetailsDTO(
        int OrderId,
        DateTime OrderDate,
        float OrderSum,
        int OrderItemsCount,
        string StatusName,
        int UserId,
        int ReviewId,
        string ReviewImageUrl,
        int Score,
        string SiteName,
        string SiteTypeName,
        string SiteDescription,
        List<OrderItemDTO> Items
    ) : OrderSummaryDTO(OrderId, OrderDate, OrderSum, StatusName, OrderItemsCount);

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
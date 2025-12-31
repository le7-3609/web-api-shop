using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CartDTO(
        int CartId,
        int UserId,
        List<CartItemDTO> CartItems,
        float TotalPrice,
        int BasicSiteId,
        string BasicSiteName,
        string BasicSiteUserDescription
    );

    public record CartItemDTO(
        int CartItemId,
        int CartId,
        string SubCategoryName,
        string ProductName,
        string ImageUrl,
        string SubCategoryDescription,
        float Price,
        string PlatformName,
        string UserDescription,
        bool IsActive,
        int ProductId,  
        int PlatformId
    );

    public record AddCartItemDTO(
        int CartId,
        int ProductId,
        int BasicSitesPlatformId,
        string UserDescription
    );

    public record AddCartDTO(
        int UserId,
        int BasicSiteId
    );
}

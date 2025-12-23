using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CartItemDTO(
        int CartId,
        int UserId,
        string SubCategoryName,
        string ProductName,
        string ImageUrl,
        string SubCategoryDescription,
        float Price,
        string PlatformName,
        string UserDescription,
        bool IsActive
    );

    public record AddCartItemDTO(
        int UserId,
        int ProductId,
        int BasicSitesPlatformId,
        string UserDescription
    );
}
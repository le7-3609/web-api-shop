using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record CartDTO
    {
        public int CartId { get; init; }
        public int UserId { get; init; }
        public List<CartItemDTO> CartItems { get; init; }
        public float TotalPrice { get; init; }
        public int BasicSiteId { get; init; }
        public string BasicSiteName { get; init; }
        public string BasicSiteUserDescription { get; init; }
    }

    public record CartItemDTO
    {
        public int CartItemId { get; init; }
        public int CartId { get; init; }
        public string SubCategoryName { get; init; }
        public string ProductName { get; init; }
        public string ImageUrl { get; init; }
        public string SubCategoryDescription { get; init; }
        public float Price { get; init; }
        public string PlatformName { get; init; }
        public string UserDescription { get; init; }
        public bool IsActive { get; init; }
        public int ProductId { get; init; }
        public int PlatformId { get; init; }
    }

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

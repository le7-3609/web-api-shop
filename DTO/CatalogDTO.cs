using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
   public record MainCategoryDTO(
        int MainCategoryId,
        string MainCategoryName
    );
    public class ManegerMainCategoryDTO
    (
        string MainCategoryName
    );

    public record SubCategoryDTO
    {
        public int SubCategoryId { get; init; }
        public int MainCategoryId { get; init; }
        public string SubCategoryName { get; init; }
        public string? ImageUrl { get; init; }
        public string CategoryDescription { get; init; }
    }

    public record AddSubCategoryDTO
    (
        [Required]
        int MainCategoryId,
        [Required]
        string SubCategoryName,
        [Required]
        string ImageUrl,
        [Required]
        string CategoryDescription
    );

 public record ProductDTO
{
    public int ProductId { get; init; }
    public int SubCategoryId { get; init; }
    public string ProductName { get; init; }
    public string SubCategoryName { get; init; }
    public float Price { get; init; }
}

    public record UpdateProductDTO
    (
        int ProductId,
        int CategoryId,
        string ProductsName,
        float Price
    );
    public record AddProductDTO
    (
        int CategoryId,
        string ProductsName,
        float Price
    );
    public record AddPlatformDTO
    (
        string PlatformName
    );
}
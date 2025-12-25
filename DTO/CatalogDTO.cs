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

    public record SubCategoryDTO(
        int SubCategoryId,
        int MainCategoryId,
        string SubCategoryName,
        string ImageUrl,
        string SubCategoryDescription
    );
    public record AddSubCategoryDTO
    (
        [Required]
        int MainCategoryId,
        [Required]
        string CategoryName,
        [Required]
        string ImgUrl,
        [Required]
        string CategoryDescreption
    );

    public record ProductDTO
    (
        int ProductId,
        int CategoryId,
        string ProductsName,
        string CategoryName,
        float Price
    );

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record ProductSummaryDTO(
        int ProductId,
        string ProductName,
        float Price,
        string SubCategoryName
    );

    public record AdminProductDetailsDTO(
        int ProductId,
        string ProductName,
        float Price,
        string SubCategoryName,
        string ProductPrompt,
        int SubCategoryId
        ): ProductSummaryDTO(ProductId, ProductName, Price, SubCategoryName);

    public record SubCategoryDTO(
        int SubCategoryId,
        int MainCategoryId,
        string SubCategoryName, 
        string ImageUrl,
        string SubCategoryDescription
        );

    public record MainCategoryDTO(
        int MainCategoryId,
        string MainCategoryName
    );
}
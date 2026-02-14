using DTO;

namespace Services
{
    public interface IProductService
    {
        Task<ProductDTO> AddProductAsync(AddProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
        Task<(IEnumerable<ProductDTO>, int TotalCount)> GetProductsAsync(int position, int skip, string? desc, int?[] subCategoryIds);
        Task<ProductDTO> GetProductByIdAsync(int Id);
        Task UpdateProductAsync(int id, UpdateProductDTO dto);
    }
}
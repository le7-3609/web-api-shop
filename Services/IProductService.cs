using DTO;

namespace Services
{
    public interface IProductService
    {
        Task<ProductDTO> AddProductAsync(AddProductDTO dto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsBySubCategoryIdAsync(int categoryId);
        Task UpdateProductAsync(int id, UpdateProductDTO dto);
    }
}
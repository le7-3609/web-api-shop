using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<(IEnumerable<Product>, int TotalCount)> GetProductsAsync(int position, int skip, string? desc, int?[] subCategoryIds);
        Task UpdateProductAsync(int id, Product product);
    }
}
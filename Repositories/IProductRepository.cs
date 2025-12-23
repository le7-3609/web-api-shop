using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsBySubCategoryIdAsync(int categoryID);
        Task UpdateProductAsync(int id, Product product);
    }
}
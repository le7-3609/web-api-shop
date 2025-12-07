using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAsync(int position, int skip, string? name, int? minPrice, int? maxPrice, int?[] categoriesId);
    }
}
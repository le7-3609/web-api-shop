using DTO;

namespace Services
{
    public interface IProductCacheService
    {
        Task<ProductDTO?> GetProductAsync(long id);
        Task SetProductAsync(long id, ProductDTO product);
        Task<(IEnumerable<ProductDTO>? Items, int TotalCount)> GetProductListAsync(string cacheKey);
        Task SetProductListAsync(string cacheKey, IEnumerable<ProductDTO> items, int totalCount);
        Task InvalidateProductAsync(long id);
        Task InvalidateProductListsAsync();
        Task<string> BuildListCacheKeyAsync(int position, int skip, string? desc, int?[] subCategoryIds);
    }
}

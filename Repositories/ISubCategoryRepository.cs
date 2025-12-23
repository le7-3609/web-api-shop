using Entities;

namespace Repositories
{
    public interface ISubCategoryRepository
    {
        Task<SubCategory> AddSubCategoryAsync(SubCategory category);
        Task<bool> DeleteSubCategoryAsync(int id);
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);
        Task UpdateSubCategoryAsync(int id, SubCategory category);
        Task<IEnumerable<SubCategory>> GetSubCategoryAsync(int paging, int limit, string? search, int? minPrice, int? MaxPrice, int? mainCategoryID);
    }
}
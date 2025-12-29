using Entities;

namespace Repositories
{
    public interface ISubCategoryRepository
    {
        Task<SubCategory> AddSubCategoryAsync(SubCategory category);
        Task<bool> DeleteSubCategoryAsync(int id);
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);
        Task UpdateSubCategoryAsync(int id, SubCategory category);
        Task<(IEnumerable<SubCategory>, int TotalCount)> GetSubCategoryAsync(int position, int skip, string? desc, int?[] mainCategoryIds);    }
}
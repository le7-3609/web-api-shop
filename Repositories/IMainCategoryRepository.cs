using Entities;

namespace Repositories
{
    public interface IMainCategoryRepository
    {
        Task<MainCategory> AddMainCategoryAsync(MainCategory mainCategoryToAdd);
        Task<bool> DeleteMainCategoryAsync(int id);
        Task<IEnumerable<MainCategory>> GetMainCategoriesAsync();
        Task<MainCategory?> GetMainCategoryByIdAsync(int id);
        Task UpdateMainCategoryAsync(MainCategory mainCategoryToUpdate);
    }
}
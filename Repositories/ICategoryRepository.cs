using Entities;

namespace Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAsync();
        Task<Category> GetByIdAsync(int id);
    }
}
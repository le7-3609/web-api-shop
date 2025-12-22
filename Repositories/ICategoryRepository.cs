using Entities;

namespace Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<MainCategory>> GetAsync();
        Task<MainCategory> GetByIdAsync(int id);
    }
}
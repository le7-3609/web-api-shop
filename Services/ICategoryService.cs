using Entities;

namespace Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAsync();
        Task<Category> GetByIdAsync(int id);
    }
}
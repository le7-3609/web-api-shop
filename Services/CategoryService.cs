using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<MainCategory> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MainCategory>> GetAsync()
        {
            return await _categoryRepository.GetAsync();
        }
    }
}

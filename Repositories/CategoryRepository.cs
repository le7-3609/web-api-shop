using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        MyShopContext _myShopContext;

        public CategoryRepository(MyShopContext context)
        {
            _myShopContext = context;
        }

        public async Task<IEnumerable<Category>> GetAsync()
        {
            return await _myShopContext.Categories.ToListAsync();
            //return await _myShopContext.Categories.Include(category => category.Products).ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _myShopContext.Categories.FirstOrDefaultAsync(category => category.CategoryId == id);
        }
    }
}
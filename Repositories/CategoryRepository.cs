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

        public async Task<IEnumerable<MainCategory>> GetAsync()
        {
            return await _myShopContext.MainCategories.ToListAsync();
            //return await _context.Categories.Include(category => category.Products).ToListAsync();
        }

        public async Task<MainCategory> GetByIdAsync(int id)
        {
            return await _myShopContext.MainCategories.FirstOrDefaultAsync(category => category.MainCategoryId == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories
{
    public class MainCategoryRepository : IMainCategoryRepository
    {
        MyShopContext _context;
        public MainCategoryRepository(MyShopContext context)
        {
            _context = context;
        }

        async public Task<IEnumerable<MainCategory>> GetMainCategoriesAsync()
        {
            return await _context.MainCategories.ToListAsync();
        }

        async public Task<MainCategory> AddMainCategoryAsync(MainCategory mainCategoryToAdd)
        {

            await _context.MainCategories.AddAsync(mainCategoryToAdd);
            _context.SaveChangesAsync();
            return mainCategoryToAdd;
        }

        async public Task UpdateMainCategoryAsync(int id, MainCategory mainCategoryToUpdate)
        {

            _context.MainCategories.Update(mainCategoryToUpdate);
            await _context.SaveChangesAsync();

        }

        async public Task<bool> DeleteMainCategoryAsync(int id)
        {
            var mainCategory = await _context.MainCategories.FirstOrDefaultAsync(x => x.MainCategoryId == id);
            var listOfForginKeyObjects = await _context.SubCategories.Where(x => x.MainCategoryId == id).ToListAsync();
            if (listOfForginKeyObjects.Count == 0 && mainCategory != null)
            {
                _context.MainCategories.Remove(mainCategory);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

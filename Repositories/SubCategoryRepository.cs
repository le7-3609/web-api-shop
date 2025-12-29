using Microsoft.EntityFrameworkCore;
using Entities;

namespace Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        MyShopContext _context;
        public SubCategoryRepository(MyShopContext context)
        {
            _context = context;
        }

        async public Task<(IEnumerable<SubCategory>, int TotalCount)> GetSubCategoryAsync(int position, int skip, string? desc, int?[] mainCategoryIds)
        {
            var query = _context.SubCategories.Where(subCategory =>
                        (desc == null ? (true) : (subCategory.CategoryDescription.Contains(desc)))
                        && ((mainCategoryIds.Length == 0) ? (true) : (mainCategoryIds.Contains(subCategory.MainCategoryId))))
                        .OrderBy(mainCategoryIds => mainCategoryIds.SubCategoryName);

            Console.WriteLine(query.ToQueryString());
            List<SubCategory> subCategories = await query
                .Skip((position - 1) * skip)
                .Take(skip)
                .Include(subCategory => subCategory.MainCategory)
                .ToListAsync();
            var total = await query.CountAsync();

            return (subCategories, total);
        }

        async public Task<SubCategory?> GetSubCategoryByIdAsync(int id)
        {
            return await _context.SubCategories.FirstOrDefaultAsync(x => x.SubCategoryId == id);

        }

        async public Task UpdateSubCategoryAsync(int id, SubCategory category)
        {
            _context.SubCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        async public Task<SubCategory> AddSubCategoryAsync(SubCategory category)
        {
            await _context.SubCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        async public Task<bool> DeleteSubCategoryAsync(int id)
        {
            var Category = await _context.SubCategories.FirstOrDefaultAsync(x => x.SubCategoryId == id);

            var listOfForginKeyObjects = await _context.Products.Where(x => x.SubCategoryId == id).ToListAsync();
            if (listOfForginKeyObjects.Count == 0 && Category != null)
            {
                _context.SubCategories.Remove(Category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }
    }
}

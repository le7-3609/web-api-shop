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

        public async Task<(IEnumerable<SubCategory>, int TotalCount)> GetSubCategoryAsync(int position, int skip, string? desc, int?[] mainCategoryIds)
        {
            var ids = mainCategoryIds ?? Array.Empty<int?>();
            var query = _context.SubCategories.AsQueryable();

            if (!string.IsNullOrEmpty(desc))
            {
                query = query.Where(s => s.CategoryDescription.Contains(desc));
            }

            if (ids.Length > 0)
            {
                query = query.Where(s => ids.Contains(s.MainCategoryId));
            }

            query = query.OrderBy(s => s.SubCategoryName);

            var total = await query.CountAsync();

            var subCategories = await query
                .Skip(skip)
                .Take(position)
                .Include(s => s.MainCategory)
                .ToListAsync();

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

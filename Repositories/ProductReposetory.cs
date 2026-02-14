using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        MyShopContext _context;
        public ProductRepository(MyShopContext context)
        {
            _context = context;
        }

        async public Task<(IEnumerable<Product>, int TotalCount)> GetProductsAsync(int position, int skip, string? desc, int?[] subCategoryIds)
        {
            var ids = subCategoryIds ?? Array.Empty<int?>();
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(desc))
            {
                query = query.Where(s => s.ProductName.Contains(desc));
            }

            if (ids.Length > 0)
            {
                query = query.Where(s => ids.Contains(s.SubCategoryId));
            }

            query = query.OrderBy(s => s.ProductName);

            var total = await query.CountAsync();

            var products = await query
                .Skip(skip)          
                .Take(position)      
                .Include(s => s.SubCategory)
                .ToListAsync();

            return (products, total);
        }

        async public Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        }

        async public Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        async public Task UpdateProductAsync(int id, Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        async public Task<bool> DeleteProductAsync(int id)
        {
            var productObjectToDelete = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(p => p.PlatformId == id);
            if (cartItem != null)
            {
                return false;
            }

            var orederItem = await _context.OrderItems.FirstOrDefaultAsync(p => p.PlatformId == id);
            if (orederItem != null)
            {
                return false;
            }
            if (productObjectToDelete == null)
            {
                return false;
            }
            _context.Products.Remove(productObjectToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

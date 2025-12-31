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

        async public Task<IEnumerable<Product>> GetProductsBySubCategoryIdAsync(int categoryID)
        {
            return await _context.Products.Include(x => x.SubCategory)
                .Where(x => x.SubCategoryId == categoryID).ToListAsync();
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
            var productObjectToDelete = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.PlatformId == id);
            if (cartItem != null)
            {
                return false;
            }

            var orederItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.PlatformId == id);
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

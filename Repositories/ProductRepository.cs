using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        MyShopContext _myShopContext;

        public ProductRepository(MyShopContext shopContext)
        {
            _myShopContext = shopContext;
        }

        public async Task<IEnumerable<Product>> GetAsync(int position, int skip, string? name, int? minPrice, int? maxPrice, int?[] categoriesId)
        {
           // var query = _myShopContext.Products.Include(p => p.Category).Where(product =>
           //         (name == null ? (true) : (product.ProductName.Contains(name)))
           //         && ((minPrice == null) ? (true) : (product.Price >= minPrice))
           //         && ((maxPrice == null) ? (true) : (product.Price <= maxPrice))
           //         && ((categoriesId.Length == 0) ? (true) : (categoriesId.Contains(product.CategoryId))))
           //         .OrderBy(product => product.Price);
           // IEnumerable<Product> products = await query.ToListAsync();
           // return products;
           return await _myShopContext.Products.ToListAsync();
        }
    }
}
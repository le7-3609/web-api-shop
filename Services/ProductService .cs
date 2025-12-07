using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAsync(int position, int skip, string? name, int? minPrice, int? maxPrice, int?[] categoriesId)
        {
            return await _productRepository.GetAsync(position, skip, name, minPrice, maxPrice, categoriesId);
        }
    }
}

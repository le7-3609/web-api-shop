using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Entities;
namespace Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductCacheService _cache;

        public ProductService(IProductRepository productRepository, IMapper mapper, IProductCacheService cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        async public Task<(IEnumerable<ProductDTO>, int TotalCount)> GetProductsAsync(int position, int skip, string? desc, int?[] subCategoryIds)
        {
            var cacheKey = await _cache.BuildListCacheKeyAsync(position, skip, desc, subCategoryIds);
            var (cachedItems, cachedTotal) = await _cache.GetProductListAsync(cacheKey);
            if (cachedItems is not null)
                return (cachedItems, cachedTotal);

            var (products, totalCount) = await _productRepository.GetProductsAsync(position, skip, desc, subCategoryIds);
            var productsRes = _mapper.Map<IEnumerable<ProductDTO>>(products);

            await _cache.SetProductListAsync(cacheKey, productsRes, totalCount);
            return (productsRes, TotalCount: totalCount);
        }

        async public Task<ProductDTO?> GetProductByIdAsync(int Id)
        {
            var cached = await _cache.GetProductAsync(Id);
            if (cached is not null)
                return cached;

            Product? product = await _productRepository.GetProductByIdAsync(Id);
            var dto = _mapper.Map<ProductDTO>(product);

            if (dto is not null)
                await _cache.SetProductAsync(Id, dto);

            return dto;
        }

        async public Task UpdateProductAsync(int id, AdminProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
            {
                throw new ArgumentException("ProductName cannot be empty");
            }

            Product product = _mapper.Map<Product>(dto);
            product.ProductId = id;
            product.ProductPrompt = string.IsNullOrWhiteSpace(dto.ProductPrompt) ? product.ProductPrompt ?? "" : dto.ProductPrompt;
            await _productRepository.UpdateProductAsync(id, product);

            await _cache.InvalidateProductAsync(id);
            await _cache.InvalidateProductListsAsync();
        }

        async public Task<ProductDTO> AddProductAsync(AdminProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
            {
                throw new ArgumentException("ProductName cannot be empty");
            }

            Product product = _mapper.Map<Product>(dto);
            product.ProductPrompt = string.IsNullOrWhiteSpace(dto.ProductPrompt) ? "" : dto.ProductPrompt;
            product = await _productRepository.AddProductAsync(product);
            var result = _mapper.Map<ProductDTO>(product);

            await _cache.InvalidateProductListsAsync();
            return result;
        }

        async public Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            if (await _productRepository.HasOrderItemsByProductIdAsync(id))
            {
                throw new InvalidOperationException("Cannot delete product that has existing orders.");
            }

            await _productRepository.RemoveCartItemsByProductIdAsync(id);
            var deleted = await _productRepository.DeleteProductAsync(id);

            if (deleted)
            {
                await _cache.InvalidateProductAsync(id);
                await _cache.InvalidateProductListsAsync();
            }

            return deleted;
        }
    }
}

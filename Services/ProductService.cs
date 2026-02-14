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
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        async public Task<(IEnumerable<ProductDTO>, int TotalCount)> GetProductsAsync(int position, int skip, string? desc, int?[] subCategoryIds)
        {
            var (products, totalCount) = await _productRepository.GetProductsAsync(position, skip, desc, subCategoryIds);
            var productsRes = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return (productsRes, TotalCount: totalCount);
        }

        async public Task<ProductDTO> GetProductByIdAsync(int Id)
        {
            Product product = await _productRepository.GetProductByIdAsync(Id);
            return _mapper.Map<ProductDTO>(product);
        }

        async public Task UpdateProductAsync(int id, UpdateProductDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);
            //למלא פרומפט עם gemini
            product.ProductPrompt = "vfsghhfg";
            await _productRepository.UpdateProductAsync(id, product);
        }


        async public Task<ProductDTO> AddProductAsync(AddProductDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);
            //למלא פרומפט עם gemini
            product.ProductPrompt = "gfasdfghfh";
            product = await _productRepository.AddProductAsync(product);
            return _mapper.Map<ProductDTO>(product);
        }

        async public Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET api/<ProductsController>/5
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAsync([FromQuery] int position, [FromQuery] int skip, [FromQuery] string? name, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoriesId)
        {
            return await _productService.GetAsync(position, skip, name, minPrice, maxPrice, categoriesId);
        }
    }
}

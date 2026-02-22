using DTO;
using System;
using System.Linq;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productsServise)
        {
            _productService = productsServise; 
        }

        // GET: api/<ProductsController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<ProductDTO>> GetProductByIdAsync(int id)
        {
            ProductDTO? product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }



        // GET: api/<ProductsController>
        [HttpGet]
        async public Task<ActionResult<PaginatedResponse<ProductDTO>>> GetProductsAsync([FromQuery] int position = 10,[FromQuery] int skip = 0,[FromQuery] string? desc = null,[FromQuery] int?[] subCategoryIds = null)
        {
            var (products, totalCount) = await _productService.GetProductsAsync(position, skip, desc, subCategoryIds);

            if (products == null || !products.Any())
                return NoContent();

            return Ok(new PaginatedResponse<ProductDTO>(products, totalCount));
        }


        // POST api/<ProductsController>
        [HttpPost]
        async public Task<ActionResult<ProductDTO>> AddProductAsync([FromBody] AdminProductDTO dto)
        {

            ProductDTO product = await _productService.AddProductAsync(dto);
            return CreatedAtAction(nameof(GetProductByIdAsync), new { id = product.ProductId }, product);

        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateProductAsync(int id, [FromBody] AdminProductDTO dto )
        {
            if (dto == null)
                return BadRequest();

            await _productService.UpdateProductAsync(id, dto);
            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        async public Task<ActionResult> DeleteProductAsync(int id)
        {

            bool flag = await _productService.DeleteProductAsync(id);
            if (flag)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
    
}

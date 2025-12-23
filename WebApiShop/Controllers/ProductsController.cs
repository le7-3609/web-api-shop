using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
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

        // GET: api/<ProductsController>
        [HttpGet]
        async public Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySubCategoryIdAsync(int categoryId)
        {
            IEnumerable<ProductDTO> productsList = await _productService.GetProductsBySubCategoryIdAsync(categoryId);
            if (productsList == null)
                return NoContent();
            return Ok(productsList);
        }

    
        // POST api/<ProductsController>
        [HttpPost]
        async public Task<ActionResult<ProductDTO>> AddProductAsync([FromBody] AddProductDTO dto)
        {

            ProductDTO product = await _productService.AddProductAsync(dto);
            return CreatedAtAction(nameof(GetProductsBySubCategoryIdAsync), new { id = product.ProductId }, product);
        
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        async public Task UpdateProductAsync(int id, [FromBody] UpdateProductDTO dto )
        {
            await _productService.UpdateProductAsync(id, dto);

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

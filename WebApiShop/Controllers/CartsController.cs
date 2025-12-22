using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : Controller
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET api/<CartsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> GetByIdAsync(int id)
        {
            CartItemDTO? cartItem = await _cartService.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound($"CartItem with ID {id} not found");
            }
            return Ok(cartItem);
        }

        // POST api/<CartsController>
        [HttpPost]
        public async Task<ActionResult<CartItemDTO>> CreateUserCartAsync([FromBody] AddCartItemDTO dto)
        {
            CartItemDTO newCartItem = await _cartService.CreateUserCartAsync(dto);
            if (newCartItem != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newCartItem.CartId }, newCartItem);
            }
            return BadRequest("Cart item already exists for this user and product.");
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCartAsync(int id)
        {
            bool succeeded = await _cartService.DeleteUserCartAsync(id);
            if (!succeeded)
            {
                return NotFound($"Item with ID {id} not found in your cart");
            }
            return NoContent();
        }

        // GET api/<CartsController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetUserCartAsync([FromQuery] int userId)
        {
            var cartItems = await _cartService.GetUserCartAsync(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound($"No cart items found for user with ID {userId}");
            }
            return Ok(cartItems);
        }

        // PUT api/<CartsController/5
        [HttpPut]
        public async Task<ActionResult<CartItemDTO>> UpdateUserCartAsync([FromBody] CartItemDTO dto)
        {
            var updatedCartItem = await _cartService.UpdateUserCartAsync(dto);
            if (updatedCartItem == null)
            {
                return NotFound($"Cart item with ID {dto.CartId} not found");
            }
            return Ok(updatedCartItem);
        }
    }
}

using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET api/Carts/items/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<CartItemDTO>> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartService.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound($"CartItem with ID {id} not found");
            }
            return Ok(cartItem);
        }

        // GET api/Carts/5/items
        [HttpGet("{cartId}/items")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _cartService.GetCartItemsByCartIdAsync(cartId);
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound($"No items found for cart ID {cartId}");
            }
            return Ok(cartItems);
        }

        // POST api/Carts/items
        [HttpPost("items")]
        public async Task<ActionResult<CartItemDTO>> AddItemToCartAsync([FromBody] AddCartItemDTO dto)
        {
            try
            {
                var newCartItem = await _cartService.AddCartItemAsync(dto);
                return CreatedAtAction(nameof(GetCartItemsByCartIdAsync), new { id = newCartItem.CartItemId }, newCartItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/Carts/items
        [HttpPut("items")]
        public async Task<ActionResult<CartItemDTO>> UpdateItemInCartAsync([FromBody] CartItemDTO dto)
        {
            var updated = await _cartService.UpdateCartItemAsync(dto);
            if (updated == null)
            {
                return NotFound($"Cart item not found");
            }
            return Ok(updated);
        }

        // DELETE api/Carts/items/5
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItemFromCartAsync(int id)
        {
            var succeeded = await _cartService.DeleteCartItemAsync(id);
            if (!succeeded)
            {
                return NotFound($"Item with ID {id} not found");
            }
            return NoContent();
        }

        // DELETE api/Carts/5/clear
        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> ClearCartAsync(int cartId)
        {
            var succeeded = await _cartService.ClearCartAsync(cartId);
            if (!succeeded)
            {
                return NotFound($"Cart with ID {cartId} could not be cleared");
            }
            return NoContent();
        }
    }
}
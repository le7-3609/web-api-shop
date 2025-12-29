using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CartRepository : ICartRepository
    {
        MyShopContext _context;

        public CartRepository(MyShopContext shopContext)
        {
            _context = shopContext;
        }

        public async Task<Cart?> GetCartByIdAsync(int cartId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task<Cart> CreateUserCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            await ClearCartItemsAsync(cartId);
            Cart? cart = await GetCartByIdAsync(cartId);
            if (cart == null)
                return false;
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(int cartItemId)
        {
            CartItem? cartItem = await GetCartItemByIdAsync(cartItemId);
            if (cartItem == null)
                return false;
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartItemsAsync(int cartId)
        {
            var items = _context.CartItems.Where(ci => ci.CartId == cartId);
            if (items == null)
                return false;
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem?> GetByCartAndProductIdAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
    }
}
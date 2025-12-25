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

        public async Task<IEnumerable<CartItem>> GetUserCartAsync(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();
        }

        public async Task<CartItem> CreateUserCartAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateUserCartAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> DeleteUserCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                return false;
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem?> GetByCartAndProductIdAsync(int userId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartId == userId && c.ProductId == productId);
        }

        public async Task<CartItem?> GetByIdAsync(int id)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartId == id);
        }
    }
}
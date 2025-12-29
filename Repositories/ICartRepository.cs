using Entities;

namespace Repositories
{
    public interface ICartRepository
    {
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<bool> ClearCartItemsAsync(int cartId);
        Task<Cart> CreateUserCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(int cartId);
        Task<bool> DeleteCartItemAsync(int cartItemId);
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task<CartItem?> GetByCartAndProductIdAsync(int cartId, int productId);
    }
}
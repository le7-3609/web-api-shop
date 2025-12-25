using Entities;

namespace Repositories
{
    public interface ICartRepository
    {
        Task<CartItem> CreateUserCartAsync(CartItem cartItem);
        Task<bool> DeleteUserCartAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetUserCartAsync(int userId);
        Task<CartItem> UpdateUserCartAsync(CartItem cartItem);
        Task<CartItem> GetByCartAndProductIdAsync(int userId, int productId);
        Task<CartItem> GetByIdAsync(int id);
    }
}
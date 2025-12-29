using DTO;

namespace Services
{
    public interface ICartService
    {
        Task<bool> ClearCartAsync(int cartId);
        Task<CartItemDTO> AddCartItemAsync(AddCartItemDTO dto);
        Task<bool> DeleteCartItemAsync(int cartItemId);
        Task<CartItemDTO?> GetCartItemByIdAsync(int id);
        Task<IEnumerable<CartItemDTO>?> GetCartItemsByCartIdAsync(int cartId);
        Task<CartItemDTO> UpdateCartItemAsync(CartItemDTO dto);
    }
}
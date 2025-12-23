using DTO;

namespace Services
{
    public interface ICartService
    {
        Task<CartItemDTO?> GetByIdAsync(int id);
        Task<CartItemDTO> CreateUserCartAsync(AddCartItemDTO dto);
        Task<bool> DeleteUserCartAsync(int cartItemId);
        Task<IEnumerable<CartItemDTO>> GetUserCartAsync(int userId);
        Task<CartItemDTO> UpdateUserCartAsync(CartItemDTO dto);
    }
}
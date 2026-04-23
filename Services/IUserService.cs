using DTO;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>?> GetAllAsync();
        Task<UserProfileDTO?> GetByIdAsync(int id);
        Task<UserProfileDTO?> LoginAsync(LoginDTO dto);
        Task<(UserProfileDTO? User, string? Error)> RegisterAsync(RegisterDTO user);
        Task<UserProfileDTO?> UpdateAsync(int id, UpdateUserDTO dto);
        Task<IEnumerable<OrderSummaryDTO>?> GetAllOrdersAsync(int userId);
        Task<UserProfileDTO?> SocialLoginAsync(SocialLoginDTO dto);
    }
}
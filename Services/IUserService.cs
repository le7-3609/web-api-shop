using DTO;
using Entities;

namespace Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserProfileDTO>?> GetAllAsync();
        Task<UserProfileDTO?> GetByIdAsync(int id);
        Task<UserProfileDTO?> LoginAsync(LoginDTO dto);
        Task<UserProfileDTO?> RegisterAsync(RegisterAndUpdateDTO user);
        Task<UserProfileDTO?> UpdateAsync(int id, RegisterAndUpdateDTO dto);
        Task<IEnumerable<OrderSummaryDTO>?> GetAllOrdersAsync(int userId);
    }
}
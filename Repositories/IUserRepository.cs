using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> LoginAsync(ExistUser oldUser);
        Task<User> RegisterAsync(User user);
        Task<User> UpdateAsync(int id, User userToUpdate);
    }
}
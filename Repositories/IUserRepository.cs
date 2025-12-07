using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        //void Delete(int id);
        Task<User> GetByIdAsync(int id);
        Task<User> LoginAsync(ExistUser oldUser);
        Task<User> RegisterAsync(User user);
        Task<User> UpdateAsync(int id, User userToUpdate);
    }
}
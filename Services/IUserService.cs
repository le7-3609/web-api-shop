using Entities;

namespace Services
{
    public interface IUserService
    {
        //void Delete(int id);
        Task<User> GetByIdAsync(int id);
        Task<User> LoginAsync(ExistUser oldUser);
        Task<User> RegisterAsync(User user);
        Task<bool> UpdateAsync(int id, User userToUpdate);
    }
}
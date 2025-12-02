using Entities;

namespace Repositories
{
    public interface IUsersRepository
    {
        //void Delete(int id);
        public Task<User> GetByIdAsync(int id);
        public Task<User> LoginAsync(ExistUser oldUser);
        public Task<User> RegisterAsync(User user);
        public Task<User> UpdateAsync(int id, User userToUpdate);
    }
}
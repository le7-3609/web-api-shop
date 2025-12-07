using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        MyShopContext _myShopContext;

        public UserRepository(MyShopContext shopContext)
        {
            _myShopContext = shopContext;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _myShopContext.Users.FirstOrDefaultAsync(user => user.UserId == id);
        }

        public async Task<User> RegisterAsync(User user)
        {
            var existingUser = await _myShopContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser != null)
            {
                return null;
            }
            await _myShopContext.AddAsync(user);
            await _myShopContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> LoginAsync(ExistUser oldUser)
        {
            return await _myShopContext.Users.FirstOrDefaultAsync(user => user.Email == oldUser.Email && user.Password == oldUser.Password);
        }

        public async Task<User> UpdateAsync(int id, User userToUpdate)
        {
            var existingUser = await _myShopContext.Users.FirstOrDefaultAsync(u => u.Email == userToUpdate.Email && u.UserId != id);

            if (existingUser != null)
            {
                return null;
            }
            _myShopContext.Users.Update(userToUpdate);
            await _myShopContext.SaveChangesAsync();
            return userToUpdate;
        }
    }
}
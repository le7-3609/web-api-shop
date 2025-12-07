using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;
        private readonly IPasswordValidityService _passwordValidityService;

        public UserService(IUserRepository usersRepository, IPasswordValidityService passwordValidityService)
        {
            _usersRepository = usersRepository;
            _passwordValidityService = passwordValidityService;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _usersRepository.GetByIdAsync(id);
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (_passwordValidityService.PasswordStrength(user.Password).strength >= 2)
            {
                return await _usersRepository.RegisterAsync(user);
            }
            return null;
        }

        public async Task<User> LoginAsync(ExistUser oldUser)
        {
            return await _usersRepository.LoginAsync(oldUser);
        }

        public async Task<bool> UpdateAsync(int id, User userToUpdate)
        {
            if (_passwordValidityService.PasswordStrength(userToUpdate.Password).strength >= 2)
            {
                var updateUser = await _usersRepository.UpdateAsync(id, userToUpdate);
                if(updateUser != null)
                    return true;
            }
            return false;
        }
    }
}

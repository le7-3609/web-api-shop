using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _iusersRepository;
        private readonly IPasswordValidityService _ipasswordValidityService;

        public UsersService(IUsersRepository usersRepository, IPasswordValidityService passwordValidityService)
        {
            _iusersRepository = usersRepository;
            _ipasswordValidityService = passwordValidityService;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _iusersRepository.GetByIdAsync(id);
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (_ipasswordValidityService.PasswordStrength(user.Password).strength >= 2)
            {
                return await _iusersRepository.RegisterAsync(user);
            }
            return null;
        }

        public async Task<User> LoginAsync(ExistUser oldUser)
        {
            return await _iusersRepository.LoginAsync(oldUser);
        }

        public async Task<bool> UpdateAsync(int id, User userToUpdate)
        {
            if (_ipasswordValidityService.PasswordStrength(userToUpdate.Password).strength >= 2)
            {
                var updateUser = await _iusersRepository.UpdateAsync(id, userToUpdate);
                if(updateUser != null)
                    return true;
            }
            return false;
        }
        //public void Delete(int id)
        //{
        //    _iusersRepository.Delete(id);
        //}
    }
}

using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UsersService
    {
        UsersRepository _usersRepository = new UsersRepository();
        PasswordValidityService _passwordValidityService = new PasswordValidityService();

        public Users GetById(int id)
        {
            return _usersRepository.GetById(id);
        }

        public Users Post(Users user)
        {
            if (_passwordValidityService.PasswordStrength(user.Password).strength >= 2)
            {
                return _usersRepository.Post(user);
            }
            return null;
        }

        public Users Login(ExistUser oldUser)
        {
            return _usersRepository.Login(oldUser);
        }

        public bool Put(int id, Users userToUpdate)
        {
            if (_passwordValidityService.PasswordStrength(userToUpdate.Password).strength >= 2)
            {
                _usersRepository.Put(id, userToUpdate);
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            _usersRepository.Delete(id);
        }
    }
}

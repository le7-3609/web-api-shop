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

        public void Put(int id, Users userToUpdate)
        {
            Users existingUser = _usersRepository.GetById(id);
            if (existingUser == null)
                throw new Exception("User not found");
            PasswordValidity result = _passwordValidityService.PasswordStrength(userToUpdate.Password);
            if (result.strength >= 2)
            {
                _usersRepository.Put(id, userToUpdate);
            }
            else
            {
                userToUpdate.Password = existingUser.Password;
                _usersRepository.Put(id, userToUpdate);
                throw new Exception("Password was not updated because it is too weak.");

            }
        }

        public void Delete(int id)
        {
            _usersRepository.Delete(id);
        }
    }
}

using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _iusersRepository ;
        private readonly IPasswordValidityService _ipasswordValidityService;

        public UsersService(IUsersRepository usersRepository, IPasswordValidityService passwordValidityService)
        {
            _iusersRepository = usersRepository;
            _ipasswordValidityService = passwordValidityService;
        }
        public Users GetById(int id)
        {
            return _iusersRepository.GetById(id);
        }

        public Users Post(Users user)
        {
            if (_ipasswordValidityService.PasswordStrength(user.Password).strength >= 2)
            {
                return _iusersRepository.Post(user);
            }
            return null;
        }

        public Users Login(ExistUser oldUser)
        {
            return _iusersRepository.Login(oldUser);
        }

        public bool Put(int id, Users userToUpdate)
        {
            if (_ipasswordValidityService.PasswordStrength(userToUpdate.Password).strength >= 2)
            {
                _iusersRepository.Put(id, userToUpdate);
                return true;
            }
            return false;
        }

        public void Delete(int id)
        {
            _iusersRepository.Delete(id);
        }
    }
}

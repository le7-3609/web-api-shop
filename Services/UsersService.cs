using Entities;
using Repositories;
using System.Text.Json;

namespace Services
{
    public class UsersService
    {
        UsersRepository _usersRepository = new UsersRepository();
        public Users UsersServiceGetById(int id)
        {
            return _usersRepository.UsersRepositoryGetById(id);
        }

        public Users UsersServicePost(Users user)
        {
            return _usersRepository.UsersRepositoryPost(user);
        }

        public Users UsersServiceLogin(ExistUser oldUser)
        {
            return _usersRepository.UsersRepositoryLogin(oldUser);
        }

        public void UsersServicePut(int id, Users userToUpdate)
        {
            _usersRepository.UsersRepositoryPut(id, userToUpdate);
        }

        public void UsersServiceDelete(int id)
        {
            _usersRepository.UsersRepositoryDelete(id);
        }
    }
}

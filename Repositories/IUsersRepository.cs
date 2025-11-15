using Entities;

namespace Repositories
{
    public interface IUsersRepository
    {
        void Delete(int id);
        Users GetById(int id);
        Users Login(ExistUser oldUser);
        Users Post(Users user);
        void Put(int id, Users userToUpdate);
    }
}
using Entities;

namespace Services
{
    public interface IUsersService
    {
        void Delete(int id);
        Users GetById(int id);
        Users Login(ExistUser oldUser);
        Users Post(Users user);
        bool Put(int id, Users userToUpdate);
    }
}
using Entities;

namespace Repositories
{
    public interface IPlatformRepository
    {
        Task<Platform> AddPlatformAsync(Platform platform);
        Task<bool> DeletePlatformAsync(int id);
        Task<IEnumerable<Platform>> GetPlatformsAsync();
        Task UpdatePlatformAsync(int id, Platform platform);
        Task<Platform> GetPlatformByNameAsync(string name);
    }
}
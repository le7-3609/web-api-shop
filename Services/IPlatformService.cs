using DTO;

namespace Services
{
    public interface IPlatformService
    {
        Task<PlatformsDTO> AddPlatformAsync(string platformName);
        Task<bool> DeletePlatformAsync(int id);
        Task<IEnumerable<PlatformsDTO>> GetPlatformsAsync();
        Task UpdatePlatformAsync(int id, PlatformsDTO dto);
    }
}
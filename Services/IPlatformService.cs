using DTO;

namespace Services
{
    public interface IPlatformService
    {
        Task<PlatformsDTO> AddPlatformAsync(AddPlatformDTO dto);
        Task<bool> DeletePlatformAsync(int id);
        Task<IEnumerable<PlatformsDTO>> GetPlatformsAsync();
        Task UpdatePlatformAsync(int id, PlatformsDTO dto);
    }
}
using DTO;

namespace Services
{
    public interface IBasicSiteService
    {
        Task<BasicSiteDTO> AddBasicSiteAsync(AddBasicSiteDTO dto);
        Task<BasicSiteDTO> GetByBasicSiteIdAsync(int id);
        Task UpdateBasicSiteAsync(int id, UpdateBasicSiteDTO dto);
    }
}
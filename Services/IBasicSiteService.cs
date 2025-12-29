using DTO;

namespace Services
{
    public interface IBasicSiteService
    {
        Task<BasicSiteDTO> AddBasicSiteAsync(AddBasicSiteDTO dto);
        Task<BasicSiteDTO> GetBasicSiteByIdAsync(int id);
        Task UpdateBasicSiteAsync(int id, UpdateBasicSiteDTO dto);
    }
}
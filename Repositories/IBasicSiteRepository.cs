using Entities;

namespace Repositories
{
    public interface IBasicSiteRepository
    {
        Task<BasicSite> AddBasicSiteAsync(BasicSite basicSite);
        Task<BasicSite?> GetBasicSiteByIdAsync(int id);
        Task UpdateBasicSiteAsync(int id, BasicSite basicSite);
    }
}
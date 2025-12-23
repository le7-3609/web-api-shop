using Entities;

namespace Repositories
{
    public interface IBasicSiteRepository
    {
        Task<BasicSite> AddBasicSiteAsync(BasicSite basicSite);
        Task<BasicSite?> GetByBasicSiteIdAsync(int id);
        Task UpdateBasicSiteAsync(int id, BasicSite basicSite);
    }
}
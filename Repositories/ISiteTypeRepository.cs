using Entities;

namespace Repositories
{
    public interface ISiteTypeRepository
    {
        Task<IEnumerable<SiteType>?> GetAllAsync();
        Task<SiteType> GetByIdAsync(int id);
        Task<SiteType> UpdateByMngAsync(SiteType siteType);
    }
}
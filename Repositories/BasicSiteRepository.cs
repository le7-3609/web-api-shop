using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BasicSiteRepository : IBasicSiteRepository
    {
        MyShopContext _context;
        public BasicSiteRepository(MyShopContext context)
        {
            _context = context;
        }

        async public Task<BasicSite?> GetBasicSiteByIdAsync(int id)
        {
            return await _context.BasicSites.Include(x => x.Platform)
                .Include(x => x.SiteType)
                .FirstOrDefaultAsync(x => x.BasicSiteId == id);

        }

        async public Task UpdateBasicSiteAsync(int id, BasicSite basicSite)
        {
            _context.BasicSites.Update(basicSite);
            await _context.SaveChangesAsync();
        }

        async public Task<BasicSite> AddBasicSiteAsync(BasicSite basicSite)
        {
            await _context.BasicSites.AddAsync(basicSite);
            await _context.SaveChangesAsync();
            return basicSite;

        }
    }
}

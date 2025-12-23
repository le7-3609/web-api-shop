
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
namespace Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        MyShopContext _context;
        public PlatformRepository(MyShopContext context)
        {
            _context = context;
        }

        async public Task<IEnumerable<Platform>> GetPlatformsAsync()
        {
            return await _context.Platforms.ToListAsync();
        }

        async public Task<Platform> AddPlatformAsync(Platform platform)
        {
            await _context.Platforms.AddAsync(platform);
            await _context.SaveChangesAsync();
            return platform;
        }

        async public Task UpdatePlatformAsync(int id, Platform platform)
        {
            _context.Platforms.Update(platform);
            await _context.SaveChangesAsync();
        }

        async public Task<bool> DeletePlatformAsync(int id)
        {
            var platformObjectToDelete = await _context.Platforms.FirstOrDefaultAsync(x => x.PlatformId == id);

            //var BasicSite = await _context.BasicSites.FirstOrDefaultAsync(x => x.BasicSitesPlatforms == id);
            //if (BasicSite != null)
            //{
            //    return false;
            //}

            //var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.BasicSitesPlatform == id);
            //if (cartItem != null)
            //{
            //    return false;
            //}

            //var orederItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.BasicSitesPlatform == id);
            //if (orederItem != null)
            //{
            //    return false;
            //}
            if (platformObjectToDelete == null)
            {
                return false;
            }
            _context.Platforms.Remove(platformObjectToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

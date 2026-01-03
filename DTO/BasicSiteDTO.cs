using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public

    record BasicSiteDTO
    {
        public int BasicSiteId { get; init; }
        public string SiteName { get; init; }
        public string UserDescreption { get; init; }
        public string SiteTypeName { get; init; }
        public string PlatformName { get; init; }
        public int PlatformId { get; init; }
        public int SiteTypeId { get; init; }
        public string SiteTypeDescreption { get; init; }
    }

    public record UpdateBasicSiteDTO
    (
        int BasicSiteId,
        string SiteName,
        string UserDescreption,
        int SiteTypeId,
        int PlatformId
    );
    public record AddBasicSiteDTO
    (
        string SiteName,
        string UserDescreption,
        int SiteTypeId,
        int PlatformId
    );
}
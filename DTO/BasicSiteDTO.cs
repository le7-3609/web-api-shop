using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record BasicSiteDTO
    (
        int BasicSiteId,
        string SiteName,
        string UserDescreption,
        string SiteTypeName,
        string PlatformName,
        int PlatformId,
        int SiteTypeId,
        string SiteTypeDescreption
    );

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
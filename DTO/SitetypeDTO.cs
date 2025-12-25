using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record SiteTypeDTO(
        int SiteTypeId,
        string SiteTypeName,
        float Price,
        string SiteTypeDescription
    );

    public record MngSiteTypeDTO(
       int SiteTypeId,
       string SiteTypeName,
       float Price,
       string SiteTypeDescription,
       string SiteTypeNamePrompt,
       string SiteTypeDescriptionPrompt
   ):SiteTypeDTO(SiteTypeId,SiteTypeName,Price,SiteTypeDescription);
    
    public record AddSiteTypeDTO
    (
        string SiteTypeName,
        float Price,
        string SiteTypeDescription,
        string SiteTypeNamePrompt,
        string SiteTypeDescriptionPrompt
    );
}
using DTO;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTypeController : ControllerBase
    {
        private readonly ISiteTypeService _siteTypeService;

        public SiteTypeController(ISiteTypeService SiteTypeService)
        {
            _siteTypeService = SiteTypeService;
        }

        // GET api/<SiteTypeController>/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteTypeDTO>?>> GetAllAsync()
        {
            var siteTypes = await _siteTypeService.GetAllAsync();
            if (siteTypes == null || !siteTypes.Any())
            {
                return NotFound($"No site types found ");
            }
            return Ok(siteTypes);
        }

        // GET api/<SiteTypeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteTypeDTO>> GetByIdAsync(int id)
        {
            return await _siteTypeService.GetByIdAsync(id);
        }

        // PUT api/<SiteTypeController>/5
        [HttpPut("{id}/manager")]
        public async Task<IActionResult> UpdateByMngAsync(int id, SiteTypeDTO dto)
        {
            var updatedSiteType = await _siteTypeService.UpdateByMngAsync(id, dto);
            if (updatedSiteType == null)
            {
                return NotFound($"Cart item with ID {dto.SiteTypeId} not found");
            }
            return Ok(updatedSiteType);
            
        }
    }
}

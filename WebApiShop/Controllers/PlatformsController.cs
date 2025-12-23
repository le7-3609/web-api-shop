using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        public PlatformsController(IPlatformService platformService) {
            _platformService = platformService;
        }

        // GET: api/<PlatformsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformsDTO>>> GetPlatformsAsync()
        {
           IEnumerable < PlatformsDTO > platformList = await _platformService.GetPlatformsAsync();
            if (platformList == null)
            {
                return NoContent();
            }
            else
                return Ok(platformList);
        }

        // POST api/<PlatformsController>
        [HttpPost]
        public async Task<ActionResult<PlatformsDTO>> AddPlatformAsync([FromBody] AddPlatformDTO platform)
        {
           PlatformsDTO PlatformForReturn= await _platformService.AddPlatformAsync(platform);
           return CreatedAtAction(nameof(GetPlatformsAsync), new { id = PlatformForReturn.PlatformId }, PlatformForReturn);
        }

        // PUT api/<PlatformsController>/5
        [HttpPut("{id}")]
        public async Task UpdatePlatformAsync(int id, [FromBody] PlatformsDTO platform)
        {
            await _platformService.UpdatePlatformAsync(id,platform);
        }

        // DELETE api/<PlatformsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool flag = await _platformService.DeletePlatformAsync(id);
            if (flag)
                return Ok();
            return BadRequest();
        }
    }
}

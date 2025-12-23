using DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordValidityController : Controller
    {
        PasswordValidityService _passwordValidityService = new PasswordValidityService();
        // POST api/<PasswordValidityController>/("passwordStrength")
        [HttpPost("passwordStrength")]
        public ActionResult<PasswordDTO> PasswordStrength([FromBody] string password)
        {
            PasswordDTO passwordValidity = _passwordValidityService.PasswordStrength(password);
            if (passwordValidity != null)
            {
                return Ok(passwordValidity);
            }
            return BadRequest(passwordValidity);
        }
    }
}

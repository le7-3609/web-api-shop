using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _usersService;

        public UsersController(IUserService usersService)
        {
            _usersService = usersService;
        }

        // GET api/<UsersController>/5
        [ActionName("GetByIdAsync")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            User user = await _usersService.GetByIdAsync(id);
            if (user == null)
            {
                return NoContent();
            }
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<User>> RegisterAsync([FromBody] User user)
        {
            User newUser = await _usersService.RegisterAsync(user);
            if (newUser != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newUser.UserId }, newUser);
            }
            return BadRequest("Too weak password or email already in use by another user.");
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync([FromBody] ExistUser oldUser)
        {
            User user = await _usersService.LoginAsync(oldUser);
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] User userToUpdate)
        {
            bool isUpdat = await _usersService.UpdateAsync(id, userToUpdate);
            if (!isUpdat)
            {
                return NoContent();
            }
            return Ok(userToUpdate);
        }

        // DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    _usersService.Delete(id);
        //}
    }
}

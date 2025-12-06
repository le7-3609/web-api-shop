using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
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
                return NotFound($"User with ID {id} not found");
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
            return Unauthorized("Invalid email or password");
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] User userToUpdate)
        {
            bool isUpdated = await _usersService.UpdateAsync(id, userToUpdate);
            if (!isUpdated)
            {
                return NotFound($"User with ID {id} not found or update failed due to weak password");
            }
            return Ok(userToUpdate);
        }

        // DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    _iusersService.Delete(id);
        //}
    }
}

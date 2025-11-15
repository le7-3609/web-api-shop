using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Repositories;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _iusersService;

        public UsersController(IUsersService usersService)
        {
            _iusersService = usersService;
        }

        // GET: api/<UsersController>
        //[HttpGet]
        //public List<Users> Get()
        //{
        //    List<Users> allUsers = new();
        //    using (StreamReader reader = System.IO.File.OpenText(_filePath))
        //    {
        //        string? currentUserInFile;
        //        while ((currentUserInFile = reader.ReadLine()) != null)
        //        {
        //            Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
        //            allUsers.Add(user);
        //        }
        //    }
        //    return allUsers;
        //}


        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<Users> Get(int id)
        {
            Users user = _iusersService.GetById(id);
            if(user == null) {
                return NoContent();
            }
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<Users> Post([FromBody] Users user)
        {
            Users newUser = _iusersService.Post(user);
            if (newUser != null)
            {
                return CreatedAtAction(nameof(Get), new { id = user.UserId }, newUser);
            }
            return BadRequest("Too weak password");
        }

        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] ExistUser oldUser)
        {
            Users user = _iusersService.Login(oldUser);
            if(user != null) {
                return Ok(user);
            }
            return NoContent();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Users userToUpdate)
        {
            bool isUpdat=_iusersService.Put(id, userToUpdate);
            if (!isUpdat)
            {
                return NoContent();
            }
            return Ok(userToUpdate);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _iusersService.Delete(id);
        }
    }
}

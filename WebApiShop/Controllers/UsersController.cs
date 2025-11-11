using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
//using WebApiShop.Controllers;
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
        UsersService _usersService = new UsersService();
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
            Users user = _usersService.UsersServiceGetById(id);
            if(user == null) {
                return NoContent();
            }
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<Users> Post([FromBody] Users user)
        {
            Users newUser = _usersService.UsersServicePost(user);
            return CreatedAtAction(nameof(Get), new { id = user.UserId }, newUser);
        }

        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] ExistUser oldUser)
        {
            Users user = _usersService.UsersServiceLogin(oldUser);
            if(user != null) {
                return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
            }
            return NoContent();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Users userToUpdate)
        {
            _usersService.UsersServicePut(id, userToUpdate);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _usersService.UsersServiceDelete(id);
        }
    }
}

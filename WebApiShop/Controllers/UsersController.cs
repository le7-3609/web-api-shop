using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Repositories;
using Services;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
<<<<<<< HEAD
        private const string UsersFilePath = "..\\WebApiShop\\usersFile.txt";

        // GET: api/<UsersController>
        [HttpGet]
        public List<Users> Get()
        {
            List<Users> allUsers = new();
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
                    allUsers.Add(user);
                }
            }
            return allUsers;
        }
=======
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
>>>>>>> layered-model

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<Users> Get(int id)
        {
<<<<<<< HEAD
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
                    if (user.UserId == id)
                        return Ok(user);
                }
            }
            return NotFound();
=======
            Users user = _iusersService.GetById(id);
            if(user == null) {
                return NoContent();
            }
            return Ok(user);
>>>>>>> layered-model
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<Users> Post([FromBody] Users user)
        {
<<<<<<< HEAD
            int numberOfUsers = System.IO.File.ReadLines(UsersFilePath).Count();
            user.UserId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(UsersFilePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
=======
            Users newUser = _iusersService.Post(user);
            if (newUser != null)
            {
                return CreatedAtAction(nameof(Get), new { id = user.UserId }, newUser);
            }
            return BadRequest("Too weak password");
>>>>>>> layered-model
        }

        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] ExistUser oldUser)
        {
<<<<<<< HEAD
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
                    if (user.Email == oldUser.Email && user.Password == oldUser.Password)
                        return Ok(user);
                }
=======
            Users user = _iusersService.Login(oldUser);
            if(user != null) {
                return Ok(user);
>>>>>>> layered-model
            }
            return Unauthorized();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Users userToUpdate)
        {
<<<<<<< HEAD
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
                    if (user.UserId == id)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText(UsersFilePath);
                text = text.Replace(textToReplace, JsonSerializer.Serialize(userToUpdate));
                System.IO.File.WriteAllText(UsersFilePath, text);
=======
            bool isUpdat=_iusersService.Put(id, userToUpdate);
            if (!isUpdat)
            {
                return NoContent();
>>>>>>> layered-model
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

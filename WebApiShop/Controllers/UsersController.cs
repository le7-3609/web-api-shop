using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApiShop.Controllers;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<Users> Get(int id)
        {
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
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<Users> Post([FromBody] Users user)
        {
            int numberOfUsers = System.IO.File.ReadLines(UsersFilePath).Count();
            user.UserId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText(UsersFilePath, userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
        }

        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] ExistUser oldUser)
        {
            using (StreamReader reader = System.IO.File.OpenText(UsersFilePath))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    Users user = JsonSerializer.Deserialize<Users>(currentUserInFile);
                    if (user.Email == oldUser.Email && user.Password == oldUser.Password)
                        return Ok(user);
                }
            }
            return Unauthorized();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Users userToUpdate)
        {
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
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
<<<<<<< HEAD
        private readonly IUsersService _usersService;
=======
        private readonly IUserService _userService;
        ILogger<UsersController> _logger;
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19

        public UsersController(IUserService usersService, ILogger<UsersController> logger)
        {
<<<<<<< HEAD
            _usersService = usersService;
=======
            _userService = usersService;
            _logger = logger;
        }

        // GET api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound($"No userOrders found ");
            }
            return Ok(users);
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetByIdAsync(int id)
        {
<<<<<<< HEAD
            User user = await _usersService.GetByIdAsync(id);
=======
            UserProfileDTO? user = await _userService.GetByIdAsync(id);
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }
            return Ok(user);
        }

        // GET api/<UsersController>/5/orders
        [HttpGet("{userId}/orders")]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetAllOrdersAsync(int userId)
        {
<<<<<<< HEAD
            User newUser = await _usersService.RegisterAsync(user);
=======
            var userOrders = await _userService.GetAllOrdersAsync(userId);
            if (userOrders == null || !userOrders.Any())
            {
                return NotFound($"No orders found for user with ID {userId}");
            }
            return Ok(userOrders);
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public async Task<ActionResult<UserProfileDTO>> RegisterAsync([FromBody] RegisterDTO dto)
        {
            UserProfileDTO newUser = await _userService.RegisterAsync(dto);
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
            if (newUser != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newUser.UserId }, newUser);
            }
            return BadRequest("Too weak password or email already in use by another dto.");
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public async Task<ActionResult<UserProfileDTO>> LoginAsync([FromBody] LoginDTO dto)
        {
<<<<<<< HEAD
            User user = await _usersService.LoginAsync(oldUser);
=======
            _logger.LogInformation($"Login attempted with User Name , {dto.Email} and password {dto.Password}");
            UserProfileDTO user = await _userService.LoginAsync(dto);
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized("Invalid email or password");
<<<<<<< HEAD
=======
        }

        // POST api/users/social-login
        [HttpPost("social-login")]
        public async Task<ActionResult<UserProfileDTO>> SocialLoginAsync([FromBody] SocialLoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Social login attempt: Provider={dto.Provider}");

            var userProfile = await _userService.SocialLoginAsync(dto);

            if (userProfile == null)
            {
                _logger.LogWarning($"Social login failed for provider: {dto.Provider}");
                return Unauthorized("Authentication failed with external provider.");
            }
            return Ok(userProfile);
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] UpdateUserDTO dto)
        {
<<<<<<< HEAD
            bool isUpdated = await _usersService.UpdateAsync(id, userToUpdate);
            if (!isUpdated)
=======
            var isUpdated = await _userService.UpdateAsync(id, dto);
            if (isUpdated == null)
>>>>>>> f4fee28bfa320ab7ed1b470055296938c241ad19
            {
                return NotFound($"User with ID {id} not found or update failed due to weak password");
            }
            return Ok(dto);
        }
    }
}

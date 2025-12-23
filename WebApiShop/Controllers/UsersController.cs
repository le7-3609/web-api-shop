using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        ILogger<UsersController> _logger;

        public UsersController(IUserService usersService, ILogger<UsersController> logger)
        {
            _userService = usersService;
            _logger = logger;
        }

        // GET api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound($"No userOrders found ");
            }
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetByIdAsync(int id)
        {
            UserProfileDTO? user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found");
            }
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public async Task<ActionResult<UserProfileDTO>> RegisterAsync([FromBody] RegisterAndUpdateDTO dto)
        {
            UserProfileDTO newUser = await _userService.RegisterAsync(dto);
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
            //_logger.LogError("\nFrom Login\n");
            _logger.LogInformation($"Login attempted with User Name , {dto.Email} and password {dto.Password}");
            UserProfileDTO user = await _userService.LoginAsync(dto);
            if (user != null)
            {
                return Ok(user);
            }
            return Unauthorized("Invalid email or password");
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RegisterAndUpdateDTO dto)
        {
            var isUpdated = await _userService.UpdateAsync(id, dto);
            if (isUpdated == null)
            {
                return NotFound($"User with ID {id} not found or update failed due to weak password");
            }
            return Ok(dto);
        }

        // GET api/<UsersController>/5/orders
        [HttpGet("{userId}/orders")]
        public async Task<ActionResult<IEnumerable<UserProfileDTO>>> GetAllOrdersAsync(int userId)
        {
            var userOrders = await _userService.GetAllOrdersAsync(userId);
            if (userOrders == null || !userOrders.Any())
            {
                return NotFound($"No orders found for user with ID {userId}");
            }
            return Ok(userOrders);
        }
    }
}

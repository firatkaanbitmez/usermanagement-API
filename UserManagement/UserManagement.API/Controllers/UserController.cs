using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Service.Services;
using Microsoft.Extensions.Logging;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with id {Id}", id);
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest createUserRequest)
        {
            var createdUser = await _userService.AddUserAsync(createUserRequest);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest updateUserRequest)
        {
            await _userService.UpdateUserAsync(updateUserRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("active-user-count")]
        public async Task<IActionResult> GetActiveUserCount()
        {
            var count = await _userService.GetActiveUserCountAsync();
            return Ok(count);
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetUsersAddedBetweenDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var users = await _userService.GetUsersAddedBetweenDatesAsync(startDate, endDate);
            return Ok(users);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var users = await _userService.GetInactiveUsersAsync();
            return Ok(users);
        }
    }
}

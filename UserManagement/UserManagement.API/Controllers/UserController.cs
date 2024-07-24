using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Service.Services;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            return await HandleRequestAsync(async () =>
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }, "fetching users");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return await HandleRequestAsync(async () =>
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found with id {Id}", id);
                    return NotFound(new { message = "User not found" });
                }
                return Ok(user);
            }, $"fetching user with id {id}");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            return await HandleRequestAsync(async () =>
            {
                var createdUser = await _userService.AddUserAsync(userDto);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }, "adding user");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDto)
        {
           

            return await HandleRequestAsync(async () =>
            {
                await _userService.UpdateUserAsync(userDto);
                return NoContent();
            }, $"updating user with id {userDto.Id}");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await HandleRequestAsync(async () =>
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }, $"deleting user with id {id}");
        }

        [HttpGet("active-user-count")]
        public async Task<IActionResult> GetActiveUserCount()
        {
            return await HandleRequestAsync(async () =>
            {
                var count = await _userService.GetActiveUserCountAsync();
                return Ok(count);
            }, "fetching active user count");
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetUsersAddedBetweenDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return await HandleRequestAsync(async () =>
            {
                var users = await _userService.GetUsersAddedBetweenDatesAsync(startDate, endDate);
                return Ok(users);
            }, $"fetching users between dates {startDate} and {endDate}");
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            return await HandleRequestAsync(async () =>
            {
                var users = await _userService.GetActiveUsersAsync();
                return Ok(users);
            }, "fetching active users");
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            return await HandleRequestAsync(async () =>
            {
                var users = await _userService.GetInactiveUsersAsync();
                return Ok(users);
            }, "fetching inactive users");
        }

        private async Task<IActionResult> HandleRequestAsync(Func<Task<IActionResult>> func, string action)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, $"Error occurred while {action}.");
                Debug.WriteLine("asdas"+ex);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

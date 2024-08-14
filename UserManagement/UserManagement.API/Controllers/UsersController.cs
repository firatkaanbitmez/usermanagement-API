using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.Request;
using UserManagement.Service.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using UserManagement.Core.DTOs.Response;
using UserManagement.Core.DTOs;
using UserManagement.Core.Responses;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : UserBaseController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null)
            {
                return ApiResponse(HttpStatusCode.NotFound, new UserDataResponse<IEnumerable<UserDTO>>(null, false));
            }
            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(users, true));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return ApiResponse(HttpStatusCode.NotFound, new UserDataResponse<UserDTO>(null, false));
            }
            return ApiResponse(new UserDataResponse<UserDTO>(user, true));
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest createUserRequest)
        {
            var response = await _userService.AddUserAsync(createUserRequest);
            if (response == null || !response.IsSuccessful)
            {
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<CreateUserResponse>(null, false));
            }
            return ApiResponse(HttpStatusCode.Created, new UserDataResponse<CreateUserResponse>(response, true));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var response = await _userService.UpdateUserAsync(updateUserRequest);
            if (response == null || !response.IsSuccessful)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
            return ApiResponse(new UserDataResponse<UpdateUserResponse>(response, true));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(new DeleteUserRequest { Id = id });
            if (response == null || !response.IsSuccessful)
            {
                return ApiResponse(HttpStatusCode.BadRequest);
            }
            return ApiResponse(new UserDataResponse<DeleteUserResponse>(response, true));
        }

        [HttpGet("active-user-count")]
        public async Task<IActionResult> GetActiveUserCount()
        {
            var count = await _userService.GetActiveUserCountAsync();
            if (count == 0)
            {
                return ApiResponse(HttpStatusCode.NoContent);
            }
            return ApiResponse(new UserDataResponse<int>(count, true));
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetUsersAddedBetweenDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var users = await _userService.GetUsersAddedBetweenDatesAsync(startDate, endDate);
            if (users == null)
            {
                return ApiResponse(HttpStatusCode.NotFound);
            }
            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(users, true));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            if (users == null)
            {
                return ApiResponse(HttpStatusCode.NoContent);
            }
            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(users, true));
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var users = await _userService.GetInactiveUsersAsync();
            if (users == null)
            {
                return ApiResponse(HttpStatusCode.NoContent);
            }
            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(users, true));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.Request;
using UserManagement.Service.Services;
using System.Net;
using UserManagement.Core.DTOs.Response;
using UserManagement.Core.DTOs;
using UserManagement.Core.Responses;
using System.Collections.Generic;

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
            var response = await _userService.GetAllUsersAsync();

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<IEnumerable<UserDTO>>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(response.Data, true));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<UserDTO>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<UserDTO>(response.Data, true));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest createUserRequest)
        {
            var response = await _userService.AddUserAsync(createUserRequest);

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<CreateUserResponse>(null, false, errors));
            }

            return ApiResponse(HttpStatusCode.Created, new UserDataResponse<CreateUserResponse>(response.Data, true));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var response = await _userService.UpdateUserAsync(updateUserRequest);

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<UpdateUserResponse>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<UpdateUserResponse>(response.Data, true));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(new DeleteUserRequest { Id = id });

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<DeleteUserResponse>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<DeleteUserResponse>(response.Data, true));
        }

        [HttpGet("active-user-count")]
        public async Task<IActionResult> GetActiveUserCount()
        {
            var response = await _userService.GetActiveUserCountAsync();

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<int>(0, false, errors));
            }

            return ApiResponse(new UserDataResponse<int>(response.Data, true));
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetUsersAddedBetweenDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _userService.GetUsersAddedBetweenDatesAsync(startDate, endDate);

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<IEnumerable<UserDTO>>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(response.Data, true));
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var response = await _userService.GetActiveUsersAsync();

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<IEnumerable<UserDTO>>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(response.Data, true));
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var response = await _userService.GetInactiveUsersAsync();

            if (!response.IsSuccessful)
            {
                var errors = new List<string>();
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    errors.Add(response.ErrorMessage);
                }
                return ApiResponse(HttpStatusCode.BadRequest, new UserDataResponse<IEnumerable<UserDTO>>(null, false, errors));
            }

            return ApiResponse(new UserDataResponse<IEnumerable<UserDTO>>(response.Data, true));
        }
    }
}

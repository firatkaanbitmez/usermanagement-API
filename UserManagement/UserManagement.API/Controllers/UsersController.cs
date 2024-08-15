using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.Request;
using UserManagement.Service.Services;
using System.Net;
using UserManagement.Core.DTOs.Response;
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
            return ApiResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return ApiResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreateUserRequest createUserRequest)
        {
            var response = await _userService.AddUserAsync(createUserRequest);
            return ApiResponse(HttpStatusCode.Created, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            var response = await _userService.UpdateUserAsync(updateUserRequest);
            return ApiResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(new DeleteUserRequest { Id = id });
            return ApiResponse(response);
        }

        [HttpGet("active-user-count")]
        public async Task<IActionResult> GetActiveUserCount()
        {
            var response = await _userService.GetActiveUserCountAsync();
            return ApiResponse(response);
        }

        [HttpGet("between")]
        public async Task<IActionResult> GetUsersAddedBetweenDates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _userService.GetUsersAddedBetweenDatesAsync(startDate, endDate);
            return ApiResponse(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var response = await _userService.GetActiveUsersAsync();
            return ApiResponse(response);
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var response = await _userService.GetInactiveUsersAsync();
            return ApiResponse(response);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.DTOs.Response;
using UserManagement.Core.Responses;

namespace UserManagement.Core.Interfaces
{
    public interface IUserService
    {
        Task<UserDataResponse<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<UserDataResponse<UserDTO>> GetUserByIdAsync(int id);
        Task<UserDataResponse<CreateUserResponse>> AddUserAsync(CreateUserRequest createUserRequest);
        Task<UserDataResponse<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest);
        Task<UserDataResponse<DeleteUserResponse>> DeleteUserAsync(DeleteUserRequest deleteUserRequest);
        Task<UserDataResponse<IEnumerable<UserDTO>>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<UserDataResponse<int>> GetActiveUserCountAsync();
        Task<UserDataResponse<IEnumerable<UserDTO>>> GetActiveUsersAsync();
        Task<UserDataResponse<IEnumerable<UserDTO>>> GetInactiveUsersAsync();
    }
}

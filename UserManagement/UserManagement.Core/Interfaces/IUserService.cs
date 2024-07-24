using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;

namespace UserManagement.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> AddUserAsync(CreateUserRequest createUserRequest);
        Task UpdateUserAsync(UpdateUserRequest updateUserRequest);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<UserDTO>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate);
        Task<int> GetActiveUserCountAsync();
        Task<IEnumerable<UserDTO>> GetActiveUsersAsync();
        Task<IEnumerable<UserDTO>> GetInactiveUsersAsync();
    }
}

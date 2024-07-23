using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using UserManagement.Core.DTOs;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users.");
                throw;
            }
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with id {Id}", id);
                throw;
            }
        }

        public async Task<UserDTO> AddUserAsync(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                user.DateAdded = DateTime.UtcNow;
                user.CreatedAt = DateTime.UtcNow;
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("User created: {User}", user);

                await _publishEndpoint.Publish(user);

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user.");
                throw;
            }
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var existingUser = await _unitOfWork.Users.GetByIdAsync(user.Id);

                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }

                user.DateAdded = existingUser.DateAdded; // DateAdded bilgisini koru
                user.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                await _publishEndpoint.Publish(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user != null)
                {
                    await _unitOfWork.Users.DeleteAsync(user);
                    await _unitOfWork.CommitAsync();

                    await _publishEndpoint.Publish(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user.");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersAddedBetweenDatesAsync(startDate, endDate);
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users between dates {StartDate} and {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            try
            {
                return await _unitOfWork.Users.GetActiveUserCountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active user count.");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetActiveUsersAsync();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching active users.");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetInactiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetInactiveUsersAsync();
                return _mapper.Map<IEnumerable<UserDTO>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching inactive users.");
                throw;
            }
        }
    }
}

using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using UserManagement.Service.Validators;

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
            var users = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> AddUserAsync(CreateUserRequest createUserRequest)
        {
     

            var user = _mapper.Map<User>(createUserRequest);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = default;
            user.IsActive = true; // Ensure IsActive is set to true
            user.IsNew = true; // New user flag

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("User created: {User}", user);

            var userDto = _mapper.Map<UserDTO>(user);
            await _publishEndpoint.Publish(userDto);

            return userDto;
        }

        public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            
            _logger.LogInformation("Fetching user with id {Id}", updateUserRequest.Id);
            var user = await _unitOfWork.Users.GetByIdAsync(updateUserRequest.Id);
            if (user == null)
            {
                _logger.LogWarning("User not found with id {Id}", updateUserRequest.Id);
                throw new Exception("User not found.");
            }

            // Clone the user object to avoid circular references
            var previousState = new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsNew = user.IsNew
            };

            _logger.LogInformation("Updating user with id {Id}", updateUserRequest.Id);
            user.FirstName = updateUserRequest.FirstName;
            user.LastName = updateUserRequest.LastName;
            user.Email = updateUserRequest.Email;
            user.IsActive = updateUserRequest.IsActive;
            user.PhoneNumber = updateUserRequest.PhoneNumber;
            user.Address = updateUserRequest.Address;
            user.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt during update
            user.IsNew = false; // User updated

            _logger.LogInformation("Saving changes for user with id {Id}", updateUserRequest.Id);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.PreviousState = previousState;
            await _publishEndpoint.Publish(userDto);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                var userDto = _mapper.Map<UserDTO>(user);
                await _publishEndpoint.Publish(userDto);
            }
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            var users = await _unitOfWork.Users.GetUsersAddedBetweenDatesAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<int> GetActiveUserCountAsync()
        {
            return await _unitOfWork.Users.GetActiveUserCountAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetActiveUsersAsync()
        {
            var users = await _unitOfWork.Users.GetActiveUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<IEnumerable<UserDTO>> GetInactiveUsersAsync()
        {
            var users = await _unitOfWork.Users.GetInactiveUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}

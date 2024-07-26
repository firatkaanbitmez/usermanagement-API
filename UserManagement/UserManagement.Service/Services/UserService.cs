using AutoMapper;
using Microsoft.Extensions.Logging;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace UserManagement.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly RabbitMQService _rabbitMQService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, RabbitMQService rabbitMQService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _rabbitMQService = rabbitMQService;
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
            user.IsActive = true;
            user.IsNew = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            var message = $"\nNew User Registration\n" +
                          $"----------------------\n" +
                          $"ID          : {user.Id}\n" +
                          $"First Name  : {user.FirstName}\n" +
                          $"Last Name   : {user.LastName}\n" +
                          $"Email       : {user.Email}\n" +
                          $"PhoneNumber : {user.PhoneNumber}\n" +
                          $"Address     : {user.Address}\n" +
                          $"Created Date: {user.CreatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                          $"Active      : {user.IsActive}\n" +
                          $"----------------------\n" +
                          "User registration processed successfully.";

            _rabbitMQService.SendMessage(message);

            return userDto;
        }

        public async Task UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(updateUserRequest.Id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

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

            user.FirstName = updateUserRequest.FirstName;
            user.LastName = updateUserRequest.LastName;
            user.Email = updateUserRequest.Email;
            user.IsActive = updateUserRequest.IsActive;
            user.PhoneNumber = updateUserRequest.PhoneNumber;
            user.Address = updateUserRequest.Address;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsNew = false;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.PreviousState = previousState;

            var changes = new List<string>();
            if (user.FirstName != userDto.PreviousState.FirstName)
                changes.Add($"First Name: {userDto.PreviousState.FirstName} -> {user.FirstName}");
            if (user.LastName != userDto.PreviousState.LastName)
                changes.Add($"Last Name: {userDto.PreviousState.LastName} -> {user.LastName}");
            if (user.Email != userDto.PreviousState.Email)
                changes.Add($"Email: {userDto.PreviousState.Email} -> {user.Email}");
            if (user.PhoneNumber != userDto.PreviousState.PhoneNumber)
                changes.Add($"Phone Number: {userDto.PreviousState.PhoneNumber} -> {user.PhoneNumber}");
            if (user.Address != userDto.PreviousState.Address)
                changes.Add($"Address: {userDto.PreviousState.Address} -> {user.Address}");
            if (user.IsActive != userDto.PreviousState.IsActive)
                changes.Add($"Active: {userDto.PreviousState.IsActive} -> {user.IsActive}");

            var message = $"\nUser Update\n" +
                          $"----------------------\n" +
                          $"ID          : {user.Id}\n" +
                          $"Changes     : \n  - {string.Join("\n  - ", changes)}\n" +
                          $"Updated Date: {user.UpdatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                          $"----------------------\n" +
                          "User update processed successfully.";

            _rabbitMQService.SendMessage(message);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                var userDto = _mapper.Map<UserDTO>(user);
                var message = $"\nUser Deletion\n" +
                              $"----------------------\n" +
                              $"ID          : {user.Id}\n" +
                              $"First Name  : {user.FirstName}\n" +
                              $"Last Name   : {user.LastName}\n" +
                              $"Email       : {user.Email}\n" +
                              $"PhoneNumber : {user.PhoneNumber}\n" +
                              $"Address     : {user.Address}\n" +
                              $"Deleted Date: {DateTime.UtcNow:dd-MM-yyyy HH:mm:ss}\n" +
                              $"----------------------\n" +
                              "User deletion processed successfully.";

                _rabbitMQService.SendMessage(message);
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

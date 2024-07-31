using AutoMapper;
using Microsoft.Extensions.Logging;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UserManagement.Core.DTOs.Response;
using UserManagement.Service.Builders;

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

        public async Task<CreateUserResponse> AddUserAsync(CreateUserRequest createUserRequest)
        {
            var user = _mapper.Map<User>(createUserRequest);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.MinValue;

            user.IsActive = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            var message = UserMessageBuilder.Create()
                                             .WithHeader("New User Registration")
                                             .WithUserDetails(userDto)
                                             .WithFooter("User registration processed successfully.")
                                             .Build();

            _rabbitMQService.SendMessage(message);

            return new CreateUserResponse { Id = user.Id };
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(updateUserRequest.Id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var previousState = _mapper.Map<UserDTO>(user);

            _mapper.Map(updateUserRequest, user);
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);

            var message = UserMessageBuilder.Create()
                                             .WithHeader("User Update")
                                             .WithChanges(userDto, previousState)
                                             .WithFooter("User update processed successfully.")
                                             .Build();

            _rabbitMQService.SendMessage(message);

            return new UpdateUserResponse { Id = user.Id };
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(deleteUserRequest.Id);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                var userDto = _mapper.Map<UserDTO>(user);
                var message = UserMessageBuilder.Create()
                                                 .WithHeader("User Deletion")
                                                 .WithUserDetails(userDto)
                                                 .WithFooter("User deletion processed successfully.")
                                                 .Build();

                _rabbitMQService.SendMessage(message);

                return new DeleteUserResponse { Id = user.Id };
            }
            else
            {
                throw new Exception("User not found.");
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

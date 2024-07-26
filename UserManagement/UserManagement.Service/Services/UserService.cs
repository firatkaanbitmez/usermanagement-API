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
            user.IsActive = true;
            user.IsNew = true;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            var message = MessageBuilder.BuildAddUserMessage(userDto);

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
            user.IsNew = false;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            var userDto = _mapper.Map<UserDTO>(user);
            userDto.PreviousState = previousState;

            var message = MessageBuilder.BuildUpdateUserMessage(userDto, previousState);

            _rabbitMQService.SendMessage(message);

            return new UpdateUserResponse { Id = user.Id };
        }

        public async Task<DeleteUserResponse> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();

                var userDto = _mapper.Map<UserDTO>(user);
                var message = MessageBuilder.BuildDeleteUserMessage(userDto);

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

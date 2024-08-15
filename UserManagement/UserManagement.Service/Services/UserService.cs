using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.DTOs.Request;
using UserManagement.Core.DTOs.Response;
using UserManagement.Core.Interfaces;
using UserManagement.Core.Responses;
using UserManagement.Core.Extensions;
using UserManagement.Service.Builders;
using UserManagement.Core.Entities;

namespace UserManagement.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly RabbitMQService _rabbitMQService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, RabbitMQService rabbitMQService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<UserDataResponse<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos.ToDataResponse();
        }

        public async Task<UserDataResponse<UserDTO>> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return UserDataResponse<UserDTO>.Fail("User not found.");
            }

            var userDto = _mapper.Map<UserDTO>(user);
            return userDto.ToDataResponse();
        }

        public async Task<UserDataResponse<CreateUserResponse>> AddUserAsync(CreateUserRequest createUserRequest)
        {
            var user = _mapper.Map<User>(createUserRequest);           

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var response = new CreateUserResponse { Id = user.Id };
            var message = UserMessageBuilder.Create()
                                             .WithHeader("New User Registration")
                                             .WithUserDetails(_mapper.Map<UserDTO>(user))
                                             .WithFooter("User registration processed successfully.")
                                             .Build();

            _rabbitMQService.SendMessage(message);
            return response.ToDataResponse();
        }

        public async Task<UserDataResponse<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(updateUserRequest.Id);
            if (user == null)
            {
                return UserDataResponse<UpdateUserResponse>.Fail("User not found.");
            }

            var previousState = _mapper.Map<UserDTO>(user);
            _mapper.Map(updateUserRequest, user);

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();

            var message = UserMessageBuilder.Create()
                                             .WithHeader("User Update")
                                             .WithChanges(_mapper.Map<UserDTO>(user), previousState)
                                             .WithFooter("User update processed successfully.")
                                             .Build();

            _rabbitMQService.SendMessage(message);
            return new UpdateUserResponse { Id = user.Id }.ToDataResponse();
        }

        public async Task<UserDataResponse<DeleteUserResponse>> DeleteUserAsync(DeleteUserRequest deleteUserRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(deleteUserRequest.Id);
            if (user == null)
            {
                return UserDataResponse<DeleteUserResponse>.Fail("User not found.");
            }

            await _unitOfWork.Users.DeleteAsync(user);
            await _unitOfWork.CommitAsync();

            var message = UserMessageBuilder.Create()
                                             .WithHeader("User Deletion")
                                             .WithUserDetails(_mapper.Map<UserDTO>(user))
                                             .WithFooter("User deletion processed successfully.")
                                             .Build();

            _rabbitMQService.SendMessage(message);
            return new DeleteUserResponse { Id = user.Id }.ToDataResponse();
        }

        public async Task<UserDataResponse<IEnumerable<UserDTO>>> GetUsersAddedBetweenDatesAsync(DateTime startDate, DateTime endDate)
        {
            var users = await _unitOfWork.Users.GetUsersAddedBetweenDatesAsync(startDate, endDate);
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos.ToDataResponse();
        }

        public async Task<UserDataResponse<int>> GetActiveUserCountAsync()
        {
            var count = await _unitOfWork.Users.GetActiveUserCountAsync();
            return count.ToDataResponse();
        }

        public async Task<UserDataResponse<IEnumerable<UserDTO>>> GetActiveUsersAsync()
        {
            var users = await _unitOfWork.Users.GetActiveUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos.ToDataResponse();
        }

        public async Task<UserDataResponse<IEnumerable<UserDTO>>> GetInactiveUsersAsync()
        {
            var users = await _unitOfWork.Users.GetInactiveUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            return userDtos.ToDataResponse();
        }
    }
}

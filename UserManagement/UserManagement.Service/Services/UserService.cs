using AutoMapper;
using UserManagement.Core.DTOs;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Service.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task AddUserAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateUserAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.CommitAsync();
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
    }
}

using UserManagement.Core.DTOs.Request;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Service.Services
{
    public class UserChangeDetector : IUserChangeDetector
    {
        public bool HasChanges(UpdateUserRequest updateUserRequest, User user)
        {
            return !(user.FirstName == updateUserRequest.FirstName &&
                     user.LastName == updateUserRequest.LastName &&
                     user.Email == updateUserRequest.Email &&
                     user.IsActive == updateUserRequest.IsActive &&
                     user.PhoneNumber == updateUserRequest.PhoneNumber &&
                     user.Address == updateUserRequest.Address);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using UserManagement.Core.DTOs.Request;

namespace UserManagement.Service.Validators
{
    public static class UserRequestValidator
    {
        public static void ValidateCreateUserRequest(CreateUserRequest request)
        {
            ValidateRequest(request);
        }

        public static void ValidateUpdateUserRequest(UpdateUserRequest request)
        {
            ValidateRequest(request);
        }

        private static void ValidateRequest(object request)
        {
            var validationContext = new ValidationContext(request);
            Validator.ValidateObject(request, validationContext, validateAllProperties: true);
        }
    }
}

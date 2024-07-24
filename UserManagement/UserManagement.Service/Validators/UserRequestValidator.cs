using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using UserManagement.Core.DTOs.Request;

namespace UserManagement.Service.Validators
{
    public static class UserRequestValidator
    {
        public static void ValidateCreateUserRequest(CreateUserRequest request)
        {
            var validationContext = new ValidationContext(request);
            Validator.ValidateObject(request, validationContext, validateAllProperties: true);
        }

        public static void ValidateUpdateUserRequest(UpdateUserRequest request)
        {
            var validationContext = new ValidationContext(request);
            Validator.ValidateObject(request, validationContext, validateAllProperties: true);
        }
    }
}

using UserManagement.Core.DTOs;
using System.Text;

namespace UserManagement.Service.Builders
{
    public class UserCreateMessageBuilder
    {
        private readonly StringBuilder _details;
        private readonly UserDTO _user;

        private UserCreateMessageBuilder(UserDTO user)
        {
            _details = new StringBuilder();
            _user = user;
        }

        public static UserCreateMessageBuilder Create(UserDTO user)
        {
            return new UserCreateMessageBuilder(user);
        }

        public UserCreateMessageBuilder WithUserDetails()
        {
            AppendLineIfNotNull("ID          : ", _user.Id)
                .AppendLineIfNotNull("First Name  : ", _user.FirstName)
                .AppendLineIfNotNull("Last Name   : ", _user.LastName)
                .AppendLineIfNotNull("Email       : ", _user.Email)
                .AppendLineIfNotNull("Phone Number: ", _user.PhoneNumber)
                .AppendLineIfNotNull("Address     : ", _user.Address)
                .AppendLineIfNotNull("Created Date: ", _user.CreatedAt?.ToString("dd-MM-yyyy HH:mm:ss"))
                .AppendLineIfNotNull("Active      : ", _user.IsActive);
            return this;
        }

        private UserCreateMessageBuilder AppendLineIfNotNull(string label, object? value)
        {
            if (value != null)
            {
                _details.AppendLine($"{label}{value}");
            }
            return this;
        }

        public string Build()
        {
            return _details.ToString();
        }
    }
}

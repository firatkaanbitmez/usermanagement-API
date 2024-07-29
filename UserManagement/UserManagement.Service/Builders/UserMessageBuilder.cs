using UserManagement.Core.DTOs;
using System.Text;

namespace UserManagement.Service.Builders
{
    public class UserMessageBuilder
    {
        private readonly StringBuilder _message;
        private readonly UserDTO _user;
        private readonly UserDTO? _previousState;

        private UserMessageBuilder(UserDTO user, UserDTO? previousState = null)
        {
            _message = new StringBuilder();
            _user = user;
            _previousState = previousState;
        }

        public static UserMessageBuilder Create(UserDTO user, UserDTO? previousState = null)
        {
            return new UserMessageBuilder(user, previousState);
        }

        public UserMessageBuilder WithHeader(string header)
        {
            _message.AppendLine($"{header}")
                    .AppendLine(new string('-', 50));
            return this;
        }

        public UserMessageBuilder WithFooter(string footer)
        {
            _message.AppendLine(new string('-', 50))
                    .AppendLine(footer);
            return this;
        }

        public UserMessageBuilder WithUserDetails()
        {
            _message.AppendLine($"ID          : {_user.Id}")
                    .AppendLine($"First Name  : {_user.FirstName}")
                    .AppendLine($"Last Name   : {_user.LastName}")
                    .AppendLine($"Email       : {_user.Email}")
                    .AppendLine($"Phone Number: {_user.PhoneNumber}")
                    .AppendLine($"Address     : {_user.Address}")
                    .AppendLine($"Created Date: {_user.CreatedAt:dd-MM-yyyy HH:mm:ss}")
                    .AppendLine($"Active      : {_user.IsActive}");
            return this;
        }

        public UserMessageBuilder WithChanges()
        {
            if (_previousState != null)
            {
                var changes = new List<string>();
                if (_user.FirstName != _previousState.FirstName)
                    changes.Add($"First Name  : {_previousState.FirstName} -> {_user.FirstName}");
                if (_user.LastName != _previousState.LastName)
                    changes.Add($"Last Name   : {_previousState.LastName} -> {_user.LastName}");
                if (_user.Email != _previousState.Email)
                    changes.Add($"Email       : {_previousState.Email} -> {_user.Email}");
                if (_user.PhoneNumber != _previousState.PhoneNumber)
                    changes.Add($"Phone Number: {_previousState.PhoneNumber} -> {_user.PhoneNumber}");
                if (_user.Address != _previousState.Address)
                    changes.Add($"Address     : {_previousState.Address} -> {_user.Address}");
                if (_user.IsActive != _previousState.IsActive)
                    changes.Add($"Active      : {_previousState.IsActive} -> {_user.IsActive}");

                _message.AppendLine($"ID          : {_user.Id}")
                        .AppendLine("Changes:")
                        .AppendLine(string.Join("\n      ", changes))
                        .AppendLine($"Updated Date: {_user.UpdatedAt:dd-MM-yyyy HH:mm:ss}");
            }
            return this;
        }

        public string Build()
        {
            return _message.ToString();
        }
    }
}

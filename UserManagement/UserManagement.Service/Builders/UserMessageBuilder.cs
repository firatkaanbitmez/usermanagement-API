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

        public UserMessageBuilder WithChanges()
        {
            if (_previousState != null)
            {
                var changes = new List<string>();

                AddChangeIfDifferent("First Name  : ", _previousState.FirstName, _user.FirstName, changes);
                AddChangeIfDifferent("Last Name   : ", _previousState.LastName, _user.LastName, changes);
                AddChangeIfDifferent("Email       : ", _previousState.Email, _user.Email, changes);
                AddChangeIfDifferent("Phone Number: ", _previousState.PhoneNumber, _user.PhoneNumber, changes);
                AddChangeIfDifferent("Address     : ", _previousState.Address, _user.Address, changes);
                AddChangeIfDifferent("Active      : ", _previousState.IsActive, _user.IsActive, changes);

                _message.AppendLine($"ID          : {_user.Id}")
                        .AppendLine("Changes:")
                        .AppendLine(string.Join("\n      ", changes))
                        .AppendLine($"Updated Date: {_user.UpdatedAt:dd-MM-yyyy HH:mm:ss}");
            }
            return this;
        }

        private UserMessageBuilder AppendLineIfNotNull(string label, object? value)
        {
            if (value != null)
            {
                _message.AppendLine($"{label}{value}");
            }
            return this;
        }

        private void AddChangeIfDifferent<T>(string label, T? oldValue, T? newValue, List<string> changes)
        {
            if (!EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                changes.Add($"{label}{oldValue} -> {newValue}");
            }
        }

        public string Build()
        {
            return _message.ToString();
        }
    }
}

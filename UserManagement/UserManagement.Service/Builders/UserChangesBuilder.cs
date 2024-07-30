using UserManagement.Core.DTOs;
using System.Text;
using System.Collections.Generic;

namespace UserManagement.Service.Builders
{
    public class UserChangesBuilder
    {
        private readonly StringBuilder _changes;
        private readonly UserDTO _user;
        private readonly UserDTO _previousState;

        private UserChangesBuilder(UserDTO user, UserDTO previousState)
        {
            _changes = new StringBuilder();
            _user = user;
            _previousState = previousState;
        }

        public static UserChangesBuilder Create(UserDTO user, UserDTO previousState)
        {
            return new UserChangesBuilder(user, previousState);
        }

        public UserChangesBuilder WithChanges()
        {
            var changes = new List<string>();

            AddChangeIfDifferent("First Name  : ", _previousState.FirstName, _user.FirstName, changes);
            AddChangeIfDifferent("Last Name   : ", _previousState.LastName, _user.LastName, changes);
            AddChangeIfDifferent("Email       : ", _previousState.Email, _user.Email, changes);
            AddChangeIfDifferent("Phone Number: ", _previousState.PhoneNumber, _user.PhoneNumber, changes);
            AddChangeIfDifferent("Address     : ", _previousState.Address, _user.Address, changes);
            AddChangeIfDifferent("Active      : ", _previousState.IsActive, _user.IsActive, changes);

            _changes.AppendLine("Changes:")
                    .AppendLine(string.Join("\n      ", changes))
                    .AppendLine($"Updated Date: {_user.UpdatedAt:dd-MM-yyyy HH:mm:ss}");

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
            return _changes.ToString();
        }
    }
}

using UserManagement.Core.DTOs;
using System.Text;

namespace UserManagement.Service.Builders
{
    public class UserMessageBuilder
    {
        private readonly StringBuilder _message;

        private UserMessageBuilder()
        {
            _message = new StringBuilder();
        }

        public static UserMessageBuilder Create()
        {
            return new UserMessageBuilder();
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

        public UserMessageBuilder WithUserDetails(UserDTO user)
        {
            var userDetails = UserDetailsBuilder.Create(user)
                                                .WithUserDetails()
                                                .Build();
            _message.AppendLine(userDetails);
            return this;
        }

        public UserMessageBuilder WithChanges(UserDTO user, UserDTO previousState)
        {
            var userChanges = UserChangesBuilder.Create(user, previousState)
                                                .WithChanges()
                                                .Build();
            _message.AppendLine(userChanges);
            return this;
        }

        public string Build()
        {
            return _message.ToString();
        }
    }
}

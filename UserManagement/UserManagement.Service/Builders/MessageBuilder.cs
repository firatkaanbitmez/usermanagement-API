using UserManagement.Core.DTOs;
using System.Text;

namespace UserManagement.Service.Builders
{
    public class MessageBuilder
    {
        private readonly StringBuilder _message;

        private MessageBuilder()
        {
            _message = new StringBuilder();
        }

        public static MessageBuilder Create()
        {
            return new MessageBuilder();
        }

        public MessageBuilder WithHeader(string header)
        {
            _message.AppendLine($"{header}")
                    .AppendLine(new string('-', 50));
            return this;
        }

        public MessageBuilder WithFooter(string footer)
        {
            _message.AppendLine(new string('-', 50))
                    .AppendLine(footer);
            return this;
        }

        public MessageBuilder WithUserDetails(UserDTO user)
        {
            var userDetails = UserCreateMessageBuilder.Create(user)
                                                .WithUserDetails()
                                                .Build();
            _message.AppendLine(userDetails);
            return this;
        }

        public MessageBuilder WithChanges(UserDTO user, UserDTO previousState)
        {
            var userChanges = UserUpdateMessageBuilder.Create(user, previousState)
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

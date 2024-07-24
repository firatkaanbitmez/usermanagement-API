using MassTransit;
using Microsoft.Extensions.Logging;
using UserManagement.Core.Entities;
using System.Threading.Tasks;

namespace UserManagement.WorkerService.Consumers
{
    public class UserConsumer : IConsumer<User>
    {
        private readonly ILogger<UserConsumer> _logger;

        public UserConsumer(ILogger<UserConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<User> context)
        {
            var user = context.Message;
            _logger.LogInformation($"\nNew User Registration ID: {user.Id}\n" +
                                   $"    First Name   : {user.FirstName}\n" +
                                   $"    Last Name    : {user.LastName}\n" +
                                   $"    Email        : {user.Email}\n" +
                                   $"    Created Date : {user.CreatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                                   $"    Active       : {user.IsActive}");
            return Task.CompletedTask;
        }
    }
}

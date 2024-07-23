﻿using MassTransit;
using Microsoft.Extensions.Logging;
using UserManagement.Core.Entities;

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
            _logger.LogInformation($"Received User: {user.FirstName} {user.LastName}, Email: {user.Email}, Date Created: {user.CreatedAt}, Active: {user.IsActive}");
            return Task.CompletedTask;
        }
    }

}

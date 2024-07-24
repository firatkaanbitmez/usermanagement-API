using MassTransit;
using Microsoft.Extensions.Logging;
using UserManagement.Core.Entities;
using System.Collections.Generic;
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

        public async Task Consume(ConsumeContext<User> context)
        {
            var user = context.Message;
            try
            {
                if (user.IsNew)
                {
                    _logger.LogInformation($"\nNew User Registration\n" +
                                           $"----------------------\n" +
                                           $"ID          : {user.Id}\n" +
                                           $"First Name  : {user.FirstName}\n" +
                                           $"Last Name   : {user.LastName}\n" +
                                           $"Email       : {user.Email}\n" +
                                           $"PhoneNumber : {user.PhoneNumber}\n" +
                                           $"Address     : {user.Address}\n" +
                                           $"Created Date: {user.CreatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                                           $"Active      : {user.IsActive}\n" +
                                           $"----------------------\n" +
                                           "User registration processed successfully.");
                }
                else if (user.PreviousState != null)
                {
                    var changes = new List<string>();
                    if (user.FirstName != user.PreviousState.FirstName)
                        changes.Add($"First Name: {user.PreviousState.FirstName} -> {user.FirstName}");
                    if (user.LastName != user.PreviousState.LastName)
                        changes.Add($"Last Name: {user.PreviousState.LastName} -> {user.LastName}");
                    if (user.Email != user.PreviousState.Email)
                        changes.Add($"Email: {user.PreviousState.Email} -> {user.Email}");
                    if (user.PhoneNumber != user.PreviousState.PhoneNumber)
                        changes.Add($"Phone Number: {user.PreviousState.PhoneNumber} -> {user.PhoneNumber}");
                    if (user.Address != user.PreviousState.Address)
                        changes.Add($"Address: {user.PreviousState.Address} -> {user.Address}");
                    if (user.IsActive != user.PreviousState.IsActive)
                        changes.Add($"Active: {user.PreviousState.IsActive} -> {user.IsActive}");

                    if (changes.Count > 0)
                    {
                        _logger.LogInformation($"\nUser Update\n" +
                                               $"----------------------\n" +
                                               $"ID          : {user.Id}\n" +
                                               $"Changes     : \n  - {string.Join("\n  - ", changes)}\n" +
                                               $"Updated Date: {user.UpdatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                                               $"----------------------\n" +
                                               "User update processed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing user registration/update.");
            }

            await Task.CompletedTask;
        }
    }
}

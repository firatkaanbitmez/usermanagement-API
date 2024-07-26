using UserManagement.Core.DTOs;
using System.Collections.Generic;

namespace UserManagement.Service
{
    public static class MessageBuilder
    {
        public static string BuildAddUserMessage(UserDTO user)
        {
            return $"\nNew User Registration\n" +
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
                   "User registration processed successfully.";
        }

        public static string BuildUpdateUserMessage(UserDTO user, UserDTO previousState)
        {
            var changes = new List<string>();
            if (user.FirstName != previousState.FirstName)
                changes.Add($"First Name: {previousState.FirstName} -> {user.FirstName}");
            if (user.LastName != previousState.LastName)
                changes.Add($"Last Name: {previousState.LastName} -> {user.LastName}");
            if (user.Email != previousState.Email)
                changes.Add($"Email: {previousState.Email} -> {user.Email}");
            if (user.PhoneNumber != previousState.PhoneNumber)
                changes.Add($"Phone Number: {previousState.PhoneNumber} -> {user.PhoneNumber}");
            if (user.Address != previousState.Address)
                changes.Add($"Address: {previousState.Address} -> {user.Address}");
            if (user.IsActive != previousState.IsActive)
                changes.Add($"Active: {previousState.IsActive} -> {user.IsActive}");

            return $"\nUser Update\n" +
                   $"----------------------\n" +
                   $"ID          : {user.Id}\n" +
                   $"Changes     : \n  - {string.Join("\n  - ", changes)}\n" +
                   $"Updated Date: {user.UpdatedAt:dd-MM-yyyy HH:mm:ss}\n" +
                   $"----------------------\n" +
                   "User update processed successfully.";
        }

        public static string BuildDeleteUserMessage(UserDTO user)
        {
            return $"\nUser Deletion\n" +
                   $"----------------------\n" +
                   $"ID          : {user.Id}\n" +
                   $"First Name  : {user.FirstName}\n" +
                   $"Last Name   : {user.LastName}\n" +
                   $"Email       : {user.Email}\n" +
                   $"PhoneNumber : {user.PhoneNumber}\n" +
                   $"Address     : {user.Address}\n" +
                   $"Deleted Date: {DateTime.UtcNow:dd-MM-yyyy HH:mm:ss}\n" +
                   $"----------------------\n" +
                   "User deletion processed successfully.";
        }
    }
}

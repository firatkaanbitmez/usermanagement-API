// CreateUserRequest.cs
namespace UserManagement.Core.DTOs.Request
{
    public class CreateUserRequest :BaseUserRequest
    {
        public string Address { get; set; } = string.Empty;
    }
}

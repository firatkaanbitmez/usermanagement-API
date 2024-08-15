// UpdateUserRequest.cs
namespace UserManagement.Core.DTOs.Request
{
    public class UpdateUserRequest :BaseUserRequest
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}

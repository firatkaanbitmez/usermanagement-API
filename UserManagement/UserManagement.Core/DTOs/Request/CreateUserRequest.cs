namespace UserManagement.Core.DTOs.Request
{
    public class CreateUserRequest :BaseUserRequest
    {
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;      
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.MinValue;
    }
}

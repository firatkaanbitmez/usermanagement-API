namespace UserManagement.Core.DTOs.Response
{
    public class UpdateUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User updated successfully.";
        public bool IsSuccessful { get; set; } = true; // Default or set based on business logic

    }
}

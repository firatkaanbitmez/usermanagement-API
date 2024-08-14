namespace UserManagement.Core.DTOs.Response
{
    public class DeleteUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User deleted successfully.";
        public bool IsSuccessful { get; set; } = true; // Default or set based on business logic

    }
}

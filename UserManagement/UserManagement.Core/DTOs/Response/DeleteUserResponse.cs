namespace UserManagement.Core.DTOs.Response
{
    public class DeleteUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User deleted successfully.";

    }
}

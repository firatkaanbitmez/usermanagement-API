namespace UserManagement.Core.DTOs.Response
{
    public class UpdateUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User updated successfully.";
    }
}

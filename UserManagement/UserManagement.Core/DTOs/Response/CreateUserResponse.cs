namespace UserManagement.Core.DTOs.Response
{
    public class CreateUserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = "User created successfully.";
        public bool IsSuccessful { get; set; } = true; // Default or set based on business logic
    }

    // Similar changes for UpdateUserResponse and DeleteUserResponse

}


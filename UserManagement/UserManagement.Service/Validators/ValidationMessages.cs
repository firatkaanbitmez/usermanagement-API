namespace UserManagement.Service.Validators
{
    public static class ValidationMessages
    {
        public const string RequiredField = "{PropertyName} is required.";
        public const string MaxLength = "{PropertyName} cannot be longer than {MaxLength} characters.";
        public const string InvalidEmail = "Invalid email address.";
        public const string InvalidPhoneNumber = "Invalid phone number.";
    }
}
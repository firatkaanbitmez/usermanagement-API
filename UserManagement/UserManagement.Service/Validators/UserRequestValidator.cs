// UserRequestValidator.cs
using FluentValidation;
using UserManagement.Core.DTOs.Request;

namespace UserManagement.Service.Validators
{
    public class UserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^(\+[0-9]{9,15})$").WithMessage("Invalid phone number.")
                .When(user => !string.IsNullOrEmpty(user.PhoneNumber));

            RuleFor(user => user.Address)
                .MaximumLength(100).WithMessage("Address cannot be longer than 100 characters.")
                .When(user => !string.IsNullOrEmpty(user.Address));
        }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(user => user.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^(\+[0-9]{9,15})$").WithMessage("Invalid phone number.")
                .When(user => !string.IsNullOrEmpty(user.PhoneNumber));

            RuleFor(user => user.Address)
                .MaximumLength(100).WithMessage("Address cannot be longer than 100 characters.")
                .When(user => !string.IsNullOrEmpty(user.Address));
        }
    }
}

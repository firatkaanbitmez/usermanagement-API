using FluentValidation;
using UserManagement.Core.DTOs.Request;

namespace UserManagement.Service.Validators
{
    public class UserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public UserRequestValidator()
        {
            Include(new BaseUserRequestValidator());

            RuleFor(user => user.Address)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(user => !string.IsNullOrEmpty(user.Address));
        }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(user => user.Id)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField);

            Include(new BaseUserRequestValidator());

            RuleFor(user => user.Address)
                .MaximumLength(100).WithMessage(ValidationMessages.MaxLength)
                .When(user => !string.IsNullOrEmpty(user.Address));
        }
    }

    public class BaseUserRequestValidator : AbstractValidator<BaseUserRequest>
    {
        public BaseUserRequestValidator()
        {
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(user => user.PhoneNumber)
                .Matches(@"^(\+[0-9]{9,15})$").WithMessage(ValidationMessages.InvalidPhoneNumber)
                .When(user => !string.IsNullOrEmpty(user.PhoneNumber));
        }
    }
}
using FluentValidation;
using UserApi.Application.DTOs;

namespace UserApi.Application.Validations
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
            RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("Email address is required.")
                    .EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 100).WithMessage("First name must be between 2 and 100 characters.");
            RuleFor(user => user.LastName)
                  .NotEmpty().WithMessage("Last name is required.")
                  .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters.");
            RuleFor(user => user.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.");
        }
    }
}

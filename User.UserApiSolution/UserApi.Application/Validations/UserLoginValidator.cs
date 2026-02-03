using FluentValidation;
using UserApi.Application.DTOs;

namespace UserApi.Application.Validations
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("UserName is required.");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}

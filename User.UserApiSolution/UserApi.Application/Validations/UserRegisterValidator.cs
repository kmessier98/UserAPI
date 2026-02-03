using FluentValidation;
using UserApi.Application.DTOs;

namespace UserApi.Application.Validations
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterValidator()
        {

        }
    }
}

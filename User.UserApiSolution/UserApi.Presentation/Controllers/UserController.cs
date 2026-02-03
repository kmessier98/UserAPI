using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Extensions;

namespace UserApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;

        public UserController(IValidator<UserRegisterDTO> validator)
        {
            _userRegisterValidator = validator;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO user) 
        {
            var validationResult = await _userRegisterValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                //return UnprocessableEntity(validationResult.errors);
                validationResult.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }


            return Ok("User registered successfully.");
        }
    }
    
}

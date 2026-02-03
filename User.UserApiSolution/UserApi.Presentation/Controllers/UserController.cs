using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;

namespace UserApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;

        [HttpPost("Register")]
        public async Task<ActionResult> Register() 
        {
            // Registration logic here
            return Ok("User registered successfully.");
        }
    }
    
}

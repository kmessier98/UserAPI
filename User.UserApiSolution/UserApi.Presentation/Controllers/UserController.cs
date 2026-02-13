using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.DTOs;
using UserApi.Application.Extensions;
using UserApi.Domain.Entities;

namespace UserApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserRegisterDTO> _userRegisterValidator;
        private readonly IValidator<UserLoginDTO> _userLoginValidator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IValidator<UserRegisterDTO> registerValidator,
                              IValidator<UserLoginDTO> loginValidator,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            _userRegisterValidator = registerValidator;
            _userLoginValidator = loginValidator;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO user) 
        {
            var validationResult = await _userRegisterValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                //return UnprocessableEntity(validationResult.errors);
                validationResult.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var identityUser = new ApplicationUser
            {
                UserName = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded ? Ok(new { message = "User registered successfully." }) : BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
        {
            var validationResult = await _userLoginValidator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync( // cela crée un cookie d'authentification pour l'utilisateur si les credentials sont valides
                user.UserName,
                user.Password,
                isPersistent: user.RememberMe,
                lockoutOnFailure: true);

            return result.Succeeded ? Ok(new { message = "User logged in successfully." }) : Unauthorized( new { message = "Invalid credentials" });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); //cela retire le cookie d'authentification du client
                                                 //(le client doit envoyer withCredentials: true
                                                 //dans sa requete pour que le cookie soit pris en compte)
            return Ok(new { message = "User logged out successfully." });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me() // Pour verifier que l'utilisateur est bien authentifié
                                  // et que le cookie d'authentification fonctionne ([Authorize])
        {
            return Ok(new { User.Identity!.Name });
        }
    }
}

using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using TopShopSeller.Core;
using TopShopSeller.Models;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserRegister> _registrationValidator;
        private readonly IValidator<UserLogin> _loginValidator;
        private readonly string _requestLocation;
        public AuthController(IUserService userService, IValidator<UserRegister> registrationValidator, IValidator<UserLogin> loginValidator, IHttpContextAccessor HttpContext)
        {
            _userService = userService;
            _registrationValidator = registrationValidator;
            _loginValidator = loginValidator;
            _requestLocation = HttpContext.HttpContext.Request.Path;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            try
            {
                ValidationResult result = await _registrationValidator.ValidateAsync(user);
                var registeredUser = await _userService.Register(user);
                if (!result.IsValid)
                {
                    return BadRequest(result);
                }
                return Created(_requestLocation, registeredUser);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            try
            {
                ValidationResult result = await _loginValidator.ValidateAsync(user);
                var loggedUser = await _userService.Login(user);
                if (!result.IsValid)
                {
                    return BadRequest(result);
                }
                return Ok(loggedUser);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}

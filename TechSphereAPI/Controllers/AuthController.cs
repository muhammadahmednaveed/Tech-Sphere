using JWTAuthentication.BusinessLayer;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProject.DataLayer;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthBL _authBL;

        public AuthController(ILogger<AuthController> logger, IAuthBL authBL,IHttpContextAccessor context)
        {
            _context = context;
            _logger = logger;
            _authBL = authBL;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegister user)
        {
            try
            {
                if (await _authBL.Register(user))
                {
                    _logger.LogInformation("User Registered Successfully", user);
                    return Ok();
                }
                else
                {
                    _logger.LogError("No row affected to register");
                    return Problem("Cannot register", null, 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                return Problem(ex.Message, null, 500);
            }
          
            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin user)
        {
            using (LogContext.PushProperty("incominguser", user.Username))
                try
            {
                _logger.LogInformation("trying to log in");
                var loggedUser = await _authBL.Login(user);
                
                if (loggedUser.UserId== null)
                {
                    _logger.LogInformation("User not found");
                    return BadRequest("Bad Login Request");
                }
                else if (loggedUser.UserId==-1)
                {
                    using (LogContext.PushProperty("username", loggedUser.Username))
                    _logger.LogInformation("Incorrect password");
                    return BadRequest("Bad Login Request");
                }
                else
                {
                    using (LogContext.PushProperty("username", loggedUser.Username))
                    using (LogContext.PushProperty("token", loggedUser.Token))
                    _logger.LogInformation("password correct and token issued");
                    return Ok(loggedUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem(ex.Message, null, 500);
            }

        }


        

    }
}

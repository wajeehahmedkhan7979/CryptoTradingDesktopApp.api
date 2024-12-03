using CryptoTradingDesktopApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Models;
using System.Threading.Tasks;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register  
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(model);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(new { result.Message, result.Errors });
        }

        // POST: api/user/login  
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _userService.LoginUserAsync(model);
            if (!string.IsNullOrEmpty(token))
                return Ok(new { Token = token });

            return Unauthorized("Invalid credentials");
        }
    }
}

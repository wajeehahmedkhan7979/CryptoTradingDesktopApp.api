using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Services;
using System.Threading.Tasks;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CryptoTradingDesktopApp.Api.Models.UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);
            if (result.Success)
                return Ok(new { result.Token });

            return Unauthorized(new { result.Message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CryptoTradingDesktopApp.Api.Models.UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);
            if (result.Success)
                return Ok(new { Message = result.Message });

            return BadRequest(new { Message = result.Message });
        }
    }
}
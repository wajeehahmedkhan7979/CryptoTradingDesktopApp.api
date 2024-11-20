using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CryptoTradingDesktopApp.Api.Services;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var userId = Guid.Parse(User.FindFirst("id")?.Value);
            var wallet = await _walletService.GetWalletAsync(userId);
            return Ok(wallet);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserId = Guid.Parse(User.FindFirst("id")?.Value);
            var result = await _walletService.DepositAsync(request);
            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.UserId = Guid.Parse(User.FindFirst("id")?.Value);
            var result = await _walletService.WithdrawAsync(request);
            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
}
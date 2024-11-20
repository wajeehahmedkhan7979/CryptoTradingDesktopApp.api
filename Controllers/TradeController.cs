using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = Guid.Parse(User.FindFirst("id")?.Value);
            request.UserId = userId;

            var result = await _tradeService.PlaceOrderAsync(request);
            if (result.Success)
                return Ok(new { Message = result.Message });

            return BadRequest(new { Error = result.Message });
        }

        [HttpGet("order-book")]
        public async Task<IActionResult> GetOrderBook()
        {
            var orderBook = await _tradeService.GetOrderBookAsync();
            return Ok(orderBook);
        }

        [HttpGet("user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = Guid.Parse(User.FindFirst("id")?.Value);
            var orders = await _tradeService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
    }
}
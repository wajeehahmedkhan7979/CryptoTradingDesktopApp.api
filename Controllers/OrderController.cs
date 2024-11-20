using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CryptoTradingDesktopApp.Api.Data;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly CryptoDbContext _context;

        public OrderController(CryptoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return Ok(order);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using CryptoTradingDesktopApp.Api.Services;
using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace CryptoTradingDesktopApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetTransactionHistory(Guid userId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionHistoryByUserIdAsync(userId);

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound(new { Message = "No transactions found for this user." });
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transaction = new Transaction
                {
                    UserId = model.UserId,
                    Amount = model.Amount,
                    Currency = model.Currency,
                    TransactionType = model.TransactionType,
                    Date = DateTime.UtcNow,
                    Description = model.Description
                };

                await _transactionService.AddTransactionAsync(transaction);
                return Ok(new { Message = "Transaction added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
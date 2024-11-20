using System;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class BuyOrderRequest
    {
        public Guid UserId { get; set; }
        public required string CryptoCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
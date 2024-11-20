using System;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class OrderRequest
    {
        public Guid UserId { get; set; }
        public required string CryptoCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public OrderType Type { get; set; }
    }

}
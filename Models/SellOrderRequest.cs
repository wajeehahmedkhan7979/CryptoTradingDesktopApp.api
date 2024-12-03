using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class SellOrderRequest
    {
        public Guid UserId { get; set; }
        public string CryptoCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}
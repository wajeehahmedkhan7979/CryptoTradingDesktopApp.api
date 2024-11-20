using System;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class TradeResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Guid? TradeId { get; set; }
        public decimal? ExecutedAmount { get; set; }
        public decimal? ExecutedPrice { get; set; }
    }
}
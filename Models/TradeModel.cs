using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class TradeModel
    {
        public Guid TradeId { get; set; }
        public Guid BuyOrderId { get; set; }
        public Guid SellOrderId { get; set; }
        public required string CryptoCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime ExecutedAt { get; set; }

        public required OrderModel BuyOrder { get; set; }
        public required OrderModel SellOrder { get; set; }
    }
}
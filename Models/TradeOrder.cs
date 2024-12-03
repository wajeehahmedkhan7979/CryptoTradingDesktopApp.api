using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class TradeOrder
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public required string CryptoCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public OrderType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderType
    {
        Buy,
        Sell
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class OrderBook
    {
        public required List<OrderBookEntry> Bids { get; set; } = new List<OrderBookEntry>();
        public required List<OrderBookEntry> Asks { get; set; } = new List<OrderBookEntry>();



        public object OrderId { get; internal set; }
    }

    public class OrderBookEntry
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
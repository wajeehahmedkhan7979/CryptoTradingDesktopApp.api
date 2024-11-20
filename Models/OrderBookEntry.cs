using System.Collections.Generic;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class OrderBook
    {
        public List<OrderBookEntry> Bids { get; set; }
        public List<OrderBookEntry> Asks { get; set; }
    }

    public class OrderBookEntry
    {
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
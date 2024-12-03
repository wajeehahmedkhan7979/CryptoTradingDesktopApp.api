using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class MarketData
    {
        public string CryptoCurrency { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Volume24h { get; set; }
        public decimal PercentChange24h { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
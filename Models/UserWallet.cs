namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserWallet
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal CryptoBalance { get; set; }
        public required string Currency { get; set; }
    }
}
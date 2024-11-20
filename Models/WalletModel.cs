namespace CryptoTradingDesktopApp.Api.Models
{
    public class WalletModel
    {
        public required Guid UserId { get; set; }
        public required UserModel User { get; set; }  // Ensure navigation is set
        public required string Currency { get; set; } // Required for non-null values
        public decimal Balance { get; set; } = 0;     // Default values
        public decimal CryptoBalance { get; set; } = 0;
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class WalletModel
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; } // Foreign key to User

        public UserModel User { get; set; } = null!; // Navigation property (non-nullable)

        public string Currency { get; set; } = "USD"; // Default currency
        public decimal Balance { get; set; } = 0; // Default balance
        public decimal CryptoBalance { get; set; } = 0; // Default crypto balance
    }
}

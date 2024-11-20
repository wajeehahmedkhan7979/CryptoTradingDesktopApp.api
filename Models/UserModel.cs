namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<WalletModel> Wallets { get; set; } = new List<WalletModel>();
        public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
        public DateTime DateRegistered { get; internal set; }
        public bool IsVerified { get; internal set; }
    }
}

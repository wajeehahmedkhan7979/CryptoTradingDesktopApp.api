namespace CryptoTradingDesktopApp.Api.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid(); // Unique transaction ID
        public Guid UserId { get; set; } // User ID associated with the transaction
        public UserModel User { get; set; } // Navigation property to User
        public decimal Amount { get; set; } // Transaction amount
        public string Currency { get; set; } = "USD"; // Currency of the transaction
        public TransactionType TransactionType { get; set; } // Type of transaction (Deposit, Withdrawal, Trade)
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Transaction timestamp
        public string Description { get; set; } // Optional description of the transaction
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Trade
    }
}

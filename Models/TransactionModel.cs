// CryptoTradingDesktopApp.Api.Models/TransactionModel.cs
namespace CryptoTradingDesktopApp.Api.Models
{
    public class TransactionModel
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required UserModel User { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required TransactionType Type { get; set; }
        public required DateTime Timestamp { get; set; }
        public string? Description { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Trade
    }
}
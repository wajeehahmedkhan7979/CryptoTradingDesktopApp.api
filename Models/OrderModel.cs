// CryptoTradingDesktopApp.Api.Models/OrderModel.cs
namespace CryptoTradingDesktopApp.Api.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required UserModel User { get; set; }
        public required string CryptoCurrency { get; set; }
        public required decimal Amount { get; set; }
        public required decimal Price { get; set; }
        public required bool IsBuyOrder { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Filled,
        Cancelled
    }
}
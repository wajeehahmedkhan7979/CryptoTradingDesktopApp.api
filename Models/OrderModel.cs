namespace CryptoTradingDesktopApp.Api.Models
{
    public enum OrderStatus
    {
        Pending,
        Filled,
        Cancelled
    }

    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public UserModel User { get; set; }  // Navigation property to User
        public string CryptoCurrency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public bool IsBuyOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }  // Use OrderStatus enum
    }
}

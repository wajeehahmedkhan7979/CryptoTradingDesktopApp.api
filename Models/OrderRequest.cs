public class OrderRequest
{
    public Guid UserId { get; set; } // User placing the order
    public decimal Amount { get; set; } // Amount of cryptocurrency to buy/sell
    public decimal Price { get; set; } // Price per unit of cryptocurrency
    public OrderType Type { get; set; } // Buy or Sell order
    public string CryptoCurrency { get; set; } // The cryptocurrency (e.g., BTC, ETH)
}

public enum OrderType
{
    Buy,
    Sell
}

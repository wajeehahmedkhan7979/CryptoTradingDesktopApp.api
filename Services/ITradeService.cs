using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface ITradeService
    {
        Task<OrderBook> GetOrderBookAsync(); // Get the current order book
        Task<List<OrderModel>> GetUserOrdersAsync(Guid userId); // Get orders for a specific user
        Task<(bool Success, string Message)> PlaceOrderAsync(OrderRequest request); // Place a new order
    }
}

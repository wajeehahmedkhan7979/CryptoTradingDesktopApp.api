using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface ITradeService
    {
        Task<(bool Success, string Message)> PlaceOrderAsync(OrderRequest request);
        Task<OrderBook> GetOrderBookAsync();
        Task<List<OrderModel>> GetUserOrdersAsync(Guid userId);
    }
}
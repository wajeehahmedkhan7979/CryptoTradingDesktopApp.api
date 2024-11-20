using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderModel order);
        Task<List<OrderModel>> GetOrdersAsync(bool? isBuyOrder);
        Task DeleteOrderAsync(Guid orderId);
        Task UpdateOrderAsync(OrderModel order);
    }
}
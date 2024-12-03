using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderModel order); // Add a new order to the order book
        Task<List<OrderModel>> GetOrdersAsync(bool? isBuyOrder); // Get orders (buy or sell)
        Task DeleteOrderAsync(Guid orderId); // Delete an order from the order book
        Task UpdateOrderAsync(OrderModel order); // Update an existing order in the order book
        Task AddOrderAsync(OrderBook order); // Add a new order to Firestore
        Task<OrderBook?> GetOrderByIdAsync(Guid orderId); // Get an order from Firestore by ID
    }
}

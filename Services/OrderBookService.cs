using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly CryptoDbContext _context;

        public OrderBookService(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(OrderModel order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderModel>> GetOrdersAsync(bool? isBuyOrder)
        {
            var query = _context.Orders.AsQueryable();
            if (isBuyOrder.HasValue)
            {
                query = query.Where(o => o.IsBuyOrder == isBuyOrder.Value);
            }
            return await query.ToListAsync();
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderAsync(OrderModel order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
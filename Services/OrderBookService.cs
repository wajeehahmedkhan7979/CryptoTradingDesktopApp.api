using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using static Google.Cloud.Firestore.V1.StructuredQuery.Types;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly CryptoDbContext _context;
        private readonly FirestoreService _firestoreService;

        public OrderBookService(FirestoreService firestoreService, CryptoDbContext context)
        {
            _firestoreService = firestoreService ?? throw new ArgumentNullException(nameof(firestoreService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
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

        public async Task AddOrderAsync(OrderBook order)
        {
            var documentId = order.OrderId?.ToString() ?? throw new ArgumentNullException(nameof(order.OrderId));
            await _firestoreService.AddDocumentAsync("Orders", documentId, order);
        }

        public async Task<OrderBook?> GetOrderByIdAsync(Guid orderId)
        {
            return await _firestoreService.GetDocumentAsync<OrderBook>("OrderBooks", orderId.ToString());
        }

       

    }


}
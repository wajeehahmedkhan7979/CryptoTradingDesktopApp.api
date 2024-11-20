using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly CryptoDbContext _context;

        public TransactionService(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransactionModel>> GetTransactionHistoryByUserIdAsync(Guid userId)
        {
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task<List<TransactionModel>> GetTransactionHistoryByUserIdAsync(int userId)
        {
            var guidUserId = new Guid(userId.ToString().PadLeft(32, '0'));
            return await _context.Transactions
                .Where(t => t.UserId == guidUserId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(TransactionModel transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(transaction.UserId)
        ?? throw new InvalidOperationException("User not found");

            transaction.User = user;
            transaction.Id = Guid.NewGuid();
            transaction.Timestamp = DateTime.UtcNow;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}

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
            // Fetch transactions for the user and include the user details
            return await _context.Transactions
                .Where(t => t.UserId == userId)
                .Include(t => t.User)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(TransactionModel transaction)
        {
            // Ensure the User exists
            var user = await _context.Users.FindAsync(transaction.UserId)
                ?? throw new InvalidOperationException("User not found");

            // Set the user navigation property
            transaction.User = user;

            // Add the transaction
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
    }
}

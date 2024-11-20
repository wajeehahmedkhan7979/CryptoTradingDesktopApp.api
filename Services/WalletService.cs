// CryptoTradingDesktopApp.Api.Services/WalletService.cs
using CryptoTradingDesktopApp.Api.Data;
using CryptoTradingDesktopApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class WalletService : IWalletService
    {
        private readonly CryptoDbContext _context;

        public WalletService(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<WalletModel> GetWalletAsync(Guid userId)
        {
            var wallet = await _context.Wallets
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                var user = await _context.Users.FindAsync(userId)
                    ?? throw new InvalidOperationException($"User with ID {userId} not found.");

                wallet = new WalletModel
                {
                    UserId = userId,
                    User = user,
                    Currency = "USD",
                    Balance = 0,
                    CryptoBalance = 0
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            return wallet;
        }

        public async Task<WalletModel> GetWalletByUserIdAsync(int userId)
        {
            var guidUserId = new Guid(userId.ToString().PadLeft(32, '0'));
            return await GetWalletAsync(guidUserId);
        }

        public async Task UpdateBalanceAsync(Guid userId, decimal newBalance)
        {
            var wallet = await GetWalletAsync(userId);
            wallet.Balance = newBalance;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWalletBalanceAsync(int userId, decimal newBalance)
        {
            var guidUserId = new Guid(userId.ToString().PadLeft(32, '0'));
            await UpdateBalanceAsync(guidUserId, newBalance);
        }

        public async Task UpdateCryptoBalanceAsync(Guid userId, decimal newCryptoBalance)
        {
            var wallet = await GetWalletAsync(userId);
            wallet.CryptoBalance = newCryptoBalance;
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string Message)> DepositAsync(DepositRequest request)
        {
            try
            {
                var wallet = await GetWalletAsync(request.UserId);
                var newBalance = wallet.Balance + request.Amount;
                wallet.Balance = newBalance;
                await _context.SaveChangesAsync();
                return (true, $"Successfully deposited {request.Amount}. New balance: {newBalance}");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> WithdrawAsync(WithdrawRequest request)
        {
            try
            {
                var wallet = await GetWalletAsync(request.UserId);
                if (wallet.Balance < request.Amount)
                {
                    return (false, "Insufficient funds.");
                }

                var newBalance = wallet.Balance - request.Amount;
                wallet.Balance = newBalance;
                await _context.SaveChangesAsync();
                return (true, $"Successfully withdrew {request.Amount}. New balance: {newBalance}");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
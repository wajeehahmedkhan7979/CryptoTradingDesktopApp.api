// CryptoTradingDesktopApp.Api.Services/IWalletService.cs
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IWalletService
    {
        Task<WalletModel> GetWalletAsync(Guid userId);
        Task<WalletModel> GetWalletByUserIdAsync(int userId);
        Task UpdateBalanceAsync(Guid userId, decimal newBalance);
        Task UpdateWalletBalanceAsync(int userId, decimal newBalance);
        Task UpdateCryptoBalanceAsync(Guid userId, decimal newCryptoBalance);
        Task<(bool Success, string Message)> DepositAsync(DepositRequest request);
        Task<(bool Success, string Message)> WithdrawAsync(WithdrawRequest request);
    }
}
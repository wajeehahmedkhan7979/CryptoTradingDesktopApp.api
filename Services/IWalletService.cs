using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface IWalletService
    {
        Task<WalletModel> GetWalletAsync(Guid userId); // Get wallet for a user
        Task<WalletModel> GetWalletByUserIdAsync(int userId); // Get wallet for a user by ID (int)
        Task UpdateBalanceAsync(Guid userId, decimal newBalance); // Update the balance of a wallet
        Task UpdateWalletBalanceAsync(int userId, decimal newBalance); // Update wallet balance by user ID (int)
        Task UpdateCryptoBalanceAsync(Guid userId, decimal newCryptoBalance); // Update the crypto balance of a wallet
        Task<(bool Success, string Message)> DepositAsync(DepositRequest request); // Deposit money into a wallet
        Task<(bool Success, string Message)> WithdrawAsync(WithdrawRequest request); // Withdraw money from a wallet
    }
}

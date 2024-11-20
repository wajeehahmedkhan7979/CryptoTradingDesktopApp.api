using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> GetTransactionHistoryByUserIdAsync(Guid userId);
        Task<List<TransactionModel>> GetTransactionHistoryByUserIdAsync(int userId);
        Task AddTransactionAsync(TransactionModel transaction);
    }
}
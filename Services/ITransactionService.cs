using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionModel>> GetTransactionHistoryByUserIdAsync(Guid userId); // Get transaction history for a user
        Task AddTransactionAsync(TransactionModel transaction); // Add a new transaction
    }
}

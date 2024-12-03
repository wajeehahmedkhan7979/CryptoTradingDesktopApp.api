using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CryptoTradingDesktopApp.Api.Models
{
    public class UserModel
    {
        public required Guid UserId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;
        public bool IsVerified { get; set; } = false;

        // Relationships
        public ICollection<WalletModel> Wallets { get; set; } = new List<WalletModel>();
        public ICollection<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
        public ICollection<OrderModel> Orders { get; internal set; }
    }
}

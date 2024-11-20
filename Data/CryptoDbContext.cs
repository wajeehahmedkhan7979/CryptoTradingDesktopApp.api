using Microsoft.EntityFrameworkCore;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.Api.Data
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options) { }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<WalletModel> Wallets { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<TradeModel> Trades { get; set; }
        public DbSet<MarketData> MarketData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Wallet relationship
            modelBuilder.Entity<WalletModel>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wallets) // Ensure plural "Wallets"
                .HasForeignKey(w => w.UserId);

            // Transaction relationship
            modelBuilder.Entity<TransactionModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            // Trade relationships
            modelBuilder.Entity<TradeModel>()
                .HasOne(t => t.BuyOrder)
                .WithMany()
                .HasForeignKey(t => t.BuyOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TradeModel>()
                .HasOne(t => t.SellOrder)
                .WithMany()
                .HasForeignKey(t => t.SellOrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

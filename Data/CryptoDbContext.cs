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
        public DbSet<OrderModel> Orders { get; set; }  // Added Orders DbSet
        public DbSet<TradeModel> Trades { get; set; }
        public DbSet<MarketData> MarketData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransactionModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

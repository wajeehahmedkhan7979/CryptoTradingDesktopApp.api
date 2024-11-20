using Microsoft.EntityFrameworkCore;
using CryptoTradingDesktopApp.Api.Models;

namespace CryptoTradingDesktopApp.API.Data
{
    public class TradingDbContext : DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options) { }

        public DbSet<TradeOrder> TradeOrders { get; set; }
    }
}

using System;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Data;

namespace CryptoTradingDesktopApp.Api.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly CryptoDbContext _context;
        private Repository<UserModel>? _userRepository;
        private Repository<WalletModel>? _walletRepository;
        private Repository<TransactionModel>? _transactionRepository;
        private Repository<OrderModel>? _orderRepository;
        private Repository<TradeModel>? _tradeRepository;

        public UnitOfWork(CryptoDbContext context)
        {
            _context = context;
        }

        public Repository<UserModel> UserRepository
        {
            get
            {
                _userRepository ??= new Repository<UserModel>(_context);
                return _userRepository;
            }
        }

        public Repository<WalletModel> WalletRepository
        {
            get
            {
                _walletRepository ??= new Repository<WalletModel>(_context);
                return _walletRepository;
            }
        }

        public Repository<TransactionModel> TransactionRepository
        {
            get
            {
                _transactionRepository ??= new Repository<TransactionModel>(_context);
                return _transactionRepository;
            }
        }

        public Repository<OrderModel> OrderRepository
        {
            get
            {
                _orderRepository ??= new Repository<OrderModel>(_context);
                return _orderRepository;
            }
        }

        public Repository<TradeModel> TradeRepository
        {
            get
            {
                _tradeRepository ??= new Repository<TradeModel>(_context);
                return _tradeRepository;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
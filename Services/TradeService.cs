using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Controllers;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class TradeService : ITradeService
    {
        private readonly IOrderBookService _orderBookService;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public TradeService(
            IOrderBookService orderBookService,
            IWalletService walletService,
            ITransactionService transactionService)
        {
            _orderBookService = orderBookService;
            _walletService = walletService;
            _transactionService = transactionService;
        }

        public async Task<(bool Success, string Message)> PlaceOrderAsync(OrderRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId)
        ?? throw new InvalidOperationException("User not found");

            var order = new OrderModel
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                User = user,
                Amount = request.Amount,
                Price = request.Price,
                CryptoCurrency = request.CryptoCurrency,
                IsBuyOrder = request.Type == OrderType.Buy,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };
            var wallet = await _walletService.GetWalletAsync(request.UserId);

            if (request.Type == OrderType.Buy && wallet.Balance < request.Amount * request.Price)
            {
                return (false, "Insufficient balance to place buy order.");
            }
            else if (request.Type == OrderType.Sell && wallet.CryptoBalance < request.Amount)
            {
                return (false, "Insufficient crypto balance to place sell order.");
            }

            var order = new OrderModel
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                CryptoCurrency = request.CryptoCurrency,
                Amount = request.Amount,
                Price = request.Price,
                IsBuyOrder = request.Type == OrderType.Buy,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            await _orderBookService.AddOrderAsync(order);
            await MatchOrderAsync(order);

            return (true, $"{request.Type} order placed successfully.");
        }

        public async Task<OrderBook> GetOrderBookAsync()
        {
            var buyOrders = await _orderBookService.GetOrdersAsync(isBuyOrder: true);
            var sellOrders = await _orderBookService.GetOrdersAsync(isBuyOrder: false);

            return new OrderBook
            {
                Bids = buyOrders.Select(o => new OrderBookEntry { Price = o.Price, Amount = o.Amount }).ToList(),
                Asks = sellOrders.Select(o => new OrderBookEntry { Price = o.Price, Amount = o.Amount }).ToList()
            };
        }

        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var allOrders = await _orderBookService.GetOrdersAsync(null);
            return allOrders.Where(o => o.UserId == userId).ToList();
        }

        private async Task MatchOrderAsync(OrderModel newOrder)
        {
            var oppositeOrders = await _orderBookService.GetOrdersAsync(!newOrder.IsBuyOrder);
            oppositeOrders = oppositeOrders.OrderBy(o => o.Price).ToList();

            foreach (var oppositeOrder in oppositeOrders)
            {
                if ((newOrder.IsBuyOrder && newOrder.Price < oppositeOrder.Price) ||
                    (!newOrder.IsBuyOrder && newOrder.Price > oppositeOrder.Price))
                {
                    break;
                }

                var matchedAmount = Math.Min(newOrder.Amount, oppositeOrder.Amount);
                var matchedPrice = oppositeOrder.Price;

                await ExecuteTradeAsync(newOrder, oppositeOrder, matchedAmount, matchedPrice);

                newOrder.Amount -= matchedAmount;
                oppositeOrder.Amount -= matchedAmount;

                if (oppositeOrder.Amount == 0)
                {
                    await _orderBookService.DeleteOrderAsync(oppositeOrder.OrderId);
                }
                else
                {
                    await _orderBookService.UpdateOrderAsync(oppositeOrder);
                }

                if (newOrder.Amount == 0)
                    break;
            }

            if (newOrder.Amount > 0)
            {
                await _orderBookService.UpdateOrderAsync(newOrder);
            }
        }


        public async Task<WalletModel> GetWalletAsync(Guid userId)
        {
            var wallet = await _context.Wallets
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
                    ?? throw new InvalidOperationException($"User with ID {userId} not found.");

                wallet = new WalletModel
                {
                    UserId = userId,
                    User = user,
                    Balance = 0,
                    CryptoBalance = 0,
                    Currency = "USD"
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            return wallet;
        }

       
        private async Task ExecuteTradeAsync(OrderModel buyOrder, OrderModel sellOrder, decimal amount, decimal price)
        {
            var buyerWallet = await _walletService.GetWalletAsync(buyOrder.UserId);
            var sellerWallet = await _walletService.GetWalletAsync(sellOrder.UserId);

            await _walletService.UpdateBalanceAsync(buyOrder.UserId, buyerWallet.Balance - (amount * price));
            await _walletService.UpdateCryptoBalanceAsync(buyOrder.UserId, buyerWallet.CryptoBalance + amount);

            await _walletService.UpdateBalanceAsync(sellOrder.UserId, sellerWallet.Balance + (amount * price));
            await _walletService.UpdateCryptoBalanceAsync(sellOrder.UserId, sellerWallet.CryptoBalance - amount);

            await _transactionService.AddTransactionAsync(new TransactionModel
            {
                TransactionId = Guid.NewGuid(),
                UserId = buyOrder.UserId,
                Type = TransactionType.Trade,
                Amount = amount,
                Currency = buyOrder.CryptoCurrency,
                Timestamp = DateTime.UtcNow,
                Description = $"Bought {amount} BTC at {price} {buyOrder.CryptoCurrency}"
            });

            await _transactionService.AddTransactionAsync(new TransactionModel
            {
                TransactionId = Guid.NewGuid(),
                UserId = sellOrder.UserId,
                Type = TransactionType.Trade,
                Amount = amount * price,
                Currency = sellOrder.CryptoCurrency,
                Timestamp = DateTime.UtcNow,
                Description = $"Sold {amount} BTC at {price} {sellOrder.CryptoCurrency}"
            });
        }
    }
}
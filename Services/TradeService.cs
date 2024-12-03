using CryptoTradingDesktopApp.Api.Models;
using CryptoTradingDesktopApp.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CryptoTradingDesktopApp.Api.Services
{
    public class TradeService : ITradeService
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderBookService _orderBookService;
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public TradeService(
            IConfiguration configuration,
            IOrderBookService orderBookService,
            IWalletService walletService,
            ITransactionService transactionService)
        {
            _configuration = configuration;
            _orderBookService = orderBookService;
            _walletService = walletService;
            _transactionService = transactionService;
        }

        // Get the order book, including bids and asks (buy and sell orders)
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

        // Get orders placed by a specific user
        public async Task<List<OrderModel>> GetUserOrdersAsync(Guid userId)
        {
            var allOrders = await _orderBookService.GetOrdersAsync(null);
            return allOrders.Where(o => o.UserId == userId).ToList();
        }

        // Place a new order (either buy or sell)
        public async Task<(bool Success, string Message)> PlaceOrderAsync(OrderRequest request)
        {
            // Ensure user exists through wallet validation
            var wallet = await _walletService.GetWalletAsync(request.UserId)
                ?? throw new InvalidOperationException("User not found or wallet not initialized.");

            // Check balances for buy/sell orders
            if (request.Type == OrderType.Buy && wallet.Balance < request.Amount * request.Price)
            {
                return (false, "Insufficient balance to place buy order.");
            }
            else if (request.Type == OrderType.Sell && wallet.CryptoBalance < request.Amount)
            {
                return (false, "Insufficient crypto balance to place sell order.");
            }

            // Create and add a new order
            var order = new OrderModel
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                User = wallet.User, // Ensure User is set
                CryptoCurrency = request.CryptoCurrency,
                Amount = request.Amount,
                Price = request.Price,
                IsBuyOrder = request.Type == OrderType.Buy,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending // Order status is initially pending
            };

            // Add the order to the order book
            await _orderBookService.AddOrderAsync(order);

            // Attempt to match the new order with existing orders
            await MatchOrderAsync(order);

            return (true, $"{request.Type} order placed successfully.");
        }

        // Match the new order with the opposite orders (buy vs. sell)
        private async Task MatchOrderAsync(OrderModel newOrder)
        {
            // Find the opposite orders (buy vs sell)
            var oppositeOrders = await _orderBookService.GetOrdersAsync(!newOrder.IsBuyOrder);
            oppositeOrders = oppositeOrders.OrderBy(o => o.Price).ToList(); // Sort by price (lowest sell or highest buy)

            // Iterate over opposite orders to try to match with the new order
            foreach (var oppositeOrder in oppositeOrders)
            {
                // If the price of the new order is not favorable, stop matching
                if ((newOrder.IsBuyOrder && newOrder.Price < oppositeOrder.Price) ||
                    (!newOrder.IsBuyOrder && newOrder.Price > oppositeOrder.Price))
                {
                    break; // No further matching, as the prices are not compatible
                }

                // Match the minimum amount between the two orders
                var matchedAmount = Math.Min(newOrder.Amount, oppositeOrder.Amount);
                var matchedPrice = oppositeOrder.Price;

                // Execute the trade for the matched amount at the matched price
                await ExecuteTradeAsync(newOrder, oppositeOrder, matchedAmount, matchedPrice);

                // Update the amounts in the original orders
                newOrder.Amount -= matchedAmount;
                oppositeOrder.Amount -= matchedAmount;

                // If the opposite order is fully matched, remove it from the order book
                if (oppositeOrder.Amount == 0)
                {
                    await _orderBookService.DeleteOrderAsync(oppositeOrder.OrderId);
                }
                else
                {
                    // Otherwise, update the order in the order book
                    await _orderBookService.UpdateOrderAsync(oppositeOrder);
                }

                // If the new order is fully matched, break out of the loop
                if (newOrder.Amount == 0)
                    break;
            }

            // If the new order is partially matched or not matched, update it in the order book
            if (newOrder.Amount > 0)
            {
                await _orderBookService.UpdateOrderAsync(newOrder);
            }
        }

        // Execute the trade (transfer funds between buyer and seller and record transactions)
        private async Task ExecuteTradeAsync(OrderModel buyOrder, OrderModel sellOrder, decimal matchedAmount, decimal matchedPrice)
        {
            // Retrieve the buyer's wallet
            var buyerWallet = await _walletService.GetWalletAsync(buyOrder.UserId);
            // Retrieve the seller's wallet
            var sellerWallet = await _walletService.GetWalletAsync(sellOrder.UserId);

            // Update the buyer's wallet (decrease balance, increase crypto balance)
            await _walletService.UpdateBalanceAsync(buyOrder.UserId, buyerWallet.Balance - (matchedAmount * matchedPrice));
            await _walletService.UpdateCryptoBalanceAsync(buyOrder.UserId, buyerWallet.CryptoBalance + matchedAmount);

            // Update the seller's wallet (increase balance, decrease crypto balance)
            await _walletService.UpdateBalanceAsync(sellOrder.UserId, sellerWallet.Balance + (matchedAmount * matchedPrice));
            await _walletService.UpdateCryptoBalanceAsync(sellOrder.UserId, sellerWallet.CryptoBalance - matchedAmount);

            // Record the buyer's transaction
            await _transactionService.AddTransactionAsync(new TransactionModel
            {
                TransactionId = Guid.NewGuid(),
                UserId = buyOrder.UserId,
                TransactionType = TransactionType.Trade,
                Amount = matchedAmount,
                Currency = buyOrder.CryptoCurrency,
                Timestamp = DateTime.UtcNow,
                Description = $"Bought {matchedAmount} {buyOrder.CryptoCurrency} at {matchedPrice} per unit"
            });

            // Record the seller's transaction
            await _transactionService.AddTransactionAsync(new TransactionModel
            {
                TransactionId = Guid.NewGuid(),
                UserId = sellOrder.UserId,
                TransactionType = TransactionType.Trade,
                Amount = matchedAmount * matchedPrice, // Seller receives the currency
                Currency = sellOrder.CryptoCurrency,
                Timestamp = DateTime.UtcNow,
                Description = $"Sold {matchedAmount} {sellOrder.CryptoCurrency} at {matchedPrice} per unit"
            });
        }
    }
}

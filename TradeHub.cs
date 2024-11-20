using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CryptoTradingDesktopApp.Hubs
{
    public class TradeHub : Hub
    {
        // This method can be called by the server to notify the client
        public async Task NotifyOrderStatus(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveOrderStatus", message);
        }

        // This method is called by clients to join the trade notifications
        public async Task JoinTradeNotifications(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        // This method is called by clients to leave the trade notifications
        public async Task LeaveTradeNotifications(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
}

using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

public class TradeClient
{
    private HubConnection _connection;

    public async Task StartAsync()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://your-backend-url/tradeHub")
            .Build();

        _connection.On<string>("ReceiveTradeNotification", (message) =>
        {
            Console.WriteLine($"Trade Notification: {message}");
            // You can update the UI with trade notifications here
        });

        _connection.On<string>("ReceiveOrderNotification", (message) =>
        {
            Console.WriteLine($"Order Notification: {message}");
            // You can update the UI with order status notifications here
        });

        await _connection.StartAsync();
    }

    public async Task StopAsync()
    {
        await _connection.StopAsync();
    }
}

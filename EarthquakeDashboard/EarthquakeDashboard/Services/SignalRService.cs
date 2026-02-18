using Microsoft.AspNetCore.SignalR.Client;

namespace EarthquakeDashboard.Services;

public class SignalRService
{
    private HubConnection? _hubConnection;

    public event Action<int>? OnCounterUpdated;

    public async Task ConnectAsync(string hubUrl)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<int>("ReceiveMessage", (count) =>
        {
            OnCounterUpdated?.Invoke(count);
        });

        await _hubConnection.StartAsync();
    }

    public async Task IncrementCounter()
    {
        if (_hubConnection != null)
            await _hubConnection.SendAsync("IncrementCounter");
    }
}

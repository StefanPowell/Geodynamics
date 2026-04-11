using Microsoft.AspNetCore.SignalR.Client;

namespace EarthquakeDashboard.Services
{
    public class SignalRService
    {
        private HubConnection? _connection;

        public event Action<int>? OnCounterUpdated;

        public async Task ConnectAsync(string hubUrl)
        {
            // Prevent re-initializing if already connected
            if (_connection != null && _connection.State != HubConnectionState.Disconnected)
                return;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _connection.On<int>("CounterUpdated", (count) => OnCounterUpdated?.Invoke(count));

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                // Log this to a place you can see, like a file or the Debug console
                System.Diagnostics.Debug.WriteLine($"SignalR Error: {ex.Message}");
            }
        }


        public async Task IncrementCounter()
        {
            if (_connection != null)
            {
                await _connection.InvokeAsync("IncrementCounter");
            }
        }
    }
}
using Microsoft.AspNetCore.SignalR;

namespace EarthQuake.SignalR;

public class EarthquakeHub : Hub
{
    private static int _counter = 0; // shared across all clients

    // Called when a client clicks the increment button
    public async Task IncrementCounter()
    {
        try
        {
            var newValue = Interlocked.Increment(ref _counter);
            await Clients.All.SendAsync("CounterUpdated", newValue);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hub error in IncrementCounter: " + ex.Message);
        }
    }

    // Called automatically when a new client connects
    public override async Task OnConnectedAsync()
    {
        try
        {
            await Clients.Caller.SendAsync("CounterUpdated", _counter);

            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hub error in OnConnectedAsync: " + ex.Message);
        }
    }
}

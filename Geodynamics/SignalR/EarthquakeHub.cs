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
            _counter++;
            await Clients.All.SendAsync("ReceiveMessage", _counter);
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
            // Send current counter to only the newly connected client
            await Clients.Caller.SendAsync("ReceiveMessage", _counter);

            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hub error in OnConnectedAsync: " + ex.Message);
        }
    }
}

using EarthQuakeDataAutomate;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DataAutomate _dataAutomate;

    public Worker(ILogger<Worker> logger, DataAutomate dataAutomate)
    {
        _logger = logger;
        _dataAutomate = dataAutomate;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _dataAutomate.Start();
        _logger.LogInformation("Worker started and DataAutomate timer is running.");
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _dataAutomate.Stop();
        _logger.LogInformation("Worker stopped and DataAutomate timer is stopped.");
        return base.StopAsync(cancellationToken);
    }
}

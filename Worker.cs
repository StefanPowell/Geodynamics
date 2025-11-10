using EarthQuake.Interface.Computations.Geomorphology.FaultMechanics.StressAccumulation;

namespace EarthQuake;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;   
    private readonly IStressAccumulationService _stressAccumulationService;

    public Worker(ILogger<Worker> logger, IStressAccumulationService stressAccumulationService)
    {
        _logger = logger;
        _stressAccumulationService = stressAccumulationService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            StartAllWorkerServices();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred starting/running EarthQuake Application.");
        }
        finally
        {
            StopAllWorkerServices();
        }
    }

    public void StartAllWorkerServices()
    {
        _stressAccumulationService.Start();
    }

    public void StopAllWorkerServices()
    {
        _stressAccumulationService.Stop();
    }
}

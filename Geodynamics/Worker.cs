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
            StartAllBackGroundServices();
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
            StopAllBackGroundServices();
        }
    }

    public void StartAllBackGroundServices()
    {
        _stressAccumulationService.Start();
    }

    public void StopAllBackGroundServices()
    {
        _stressAccumulationService.Stop();
    }
}

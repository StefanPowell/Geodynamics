using EarthQuake.Models;
using EarthQuake.Persistence.Enum;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;

namespace EarthQuake.EarthquakeAnalysis;

public class QuakeAnalysis
{
    private readonly IFaultRepository _faultRepository;
    private readonly IEarthquakeRepository _quakeRepository;

    public QuakeAnalysis(IFaultRepository faultRepository, IEarthquakeRepository quakeRepository)
    {
        _faultRepository = faultRepository;
        _quakeRepository = quakeRepository;
    }

    public async Task<Fault> GetNearestFault(Feature quake)
    {
        return await _faultRepository.GetNearestFaultToCoordinates(quake.geometry.coordinates[0], quake.geometry.coordinates[1]);
    }

    public int NumberOfQuakesInTimeFrameAndWithinDistanceOfFault(DateTime starttime, DateTime endTime, double distance, UnitOfMeasure metric)
    {
        return _quakeRepository.QuakesInTimeFrameAndWithinDistanceOfFault(starttime, endTime, distance, metric);
    }

    /*
    public async Task<(int max, double mean, int min)> GetMagnitudeOfQuakesWithinDistanceOfFault(Fault fault, double distance, UnitOfMeasure metric)
    {

    }
    */
}
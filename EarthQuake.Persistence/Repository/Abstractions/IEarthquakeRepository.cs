using EarthQuake.Models;
using EarthQuake.Persistence.Enum;
using EarthQuake.Persistence.Models;

namespace EarthQuake.Persistence.Repository.Abstractions;

public interface IEarthquakeRepository
{
    Task SaveData(List<Feature> data);
    Task<List<DBFeature>> GetLatestQuakes(int totalValues);
    Task SaveWaveFormData(List<miniSEED> WaveFormData);
    Task UpdateFaultStress(FaultStress faultStress);
    int QuakesInTimeFrameAndWithinDistanceOfFault(DateTime starttime, DateTime endTime, double distance, UnitOfMeasure metric);
}

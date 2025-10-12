

using EarthQuake.Persistence.Models;

namespace EarthQuake.Persistence.Repository.Abstractions;

public interface IEarthquakeRepository
{
    Task SaveData(List<Feature> data);
    Task<List<DBFeature>> GetLatestQuakes(int totalValues);
}

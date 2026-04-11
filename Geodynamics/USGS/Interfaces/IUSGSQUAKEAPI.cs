using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Models;
using EarthQuake.Persistence.Models;

namespace EarthQuake.USGS.Interfaces
{
    public interface IUSGSQUAKEAPI
    {
        Task<int> SendQuery(DateTime startDateTime, DateTime endDateTime);
        Task<List<Feature>> GetQuakesQuery();
        Task<List<StationData>> GetStations(double latitude, double longitude, int totalstations, int maxradius);
        Task PostWaveFormData(DateTime startTime, DateTime endTime, ServiceIrisEduData data = null);
    }
}

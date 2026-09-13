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
        Task<List<miniSEED>> GetWaveFormData(DateTime startTime, DateTime endTime, string network, string station, string location = "00", string channel = "BH?", string format = "geocsv.inline");
    }
}

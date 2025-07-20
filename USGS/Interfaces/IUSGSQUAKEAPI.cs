using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Models;

namespace EarthQuake.USGS.Interfaces
{
    public interface IUSGSQUAKEAPI
    {
        void SendQuery(DateOnly starttime, DateOnly endtime);
        Task<List<Feature>> GetQuakesQuery();
        Task<List<StationData>> GetStations(double latitude, double longitude, int totalstations, int maxradius);
        Task<List<miniSEED>> GetWaveFormData(ServiceIrisEduData data = null);
    }
}

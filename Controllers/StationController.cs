using EarthQuake.Models;
using EarthQuake.USGS;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationController : ControllerBase
    {
        private readonly IUSGSQUAKEAPI  _api;

        public StationController(IUSGSQUAKEAPI api)
        {
            _api = api;
        }

        [HttpGet(Name = "NearbyStations")]
        public async Task <StationData> GetNearbyStations(double latitude, double longitude, int stationsneeded = 0, int maxradius=1)
        {
            List<StationData> allstations = await _api.GetStations(latitude, longitude, stationsneeded, maxradius);
            return allstations.First();
        }
    }
}

using EarthQuake.Models;
using EarthQuake.USGS;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationController : ControllerBase
    {
        API quakeAPI = new API();

        public StationController()
        {
            
        }

        [HttpGet(Name = "NearbyStations")]
        public async Task <StationData> GetNearbyStations(double latitude, double longitude, int stationsneeded = 0, int maxradius=1)
        {
            List<StationData> allstations = await quakeAPI.GetStations(latitude, longitude, stationsneeded, maxradius);
            return allstations.First();
        }
    }
}

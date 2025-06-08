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
        public async Task <List<StationData>> GetNearbyStations(int stationsneeded = 0)
        {
            //return new List<StationData>();
            List<StationData> allstations = await quakeAPI.GetStations();
            return allstations;
        }
    }
}

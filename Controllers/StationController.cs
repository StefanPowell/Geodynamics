using EarthQuake.Models;
using EarthQuake.USGS;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationController : ControllerBase
    {
        public StationController()
        {
            
        }

        [HttpGet(Name = "NearbyStations")]
        public List<StationData> GetQuakeData(int stationsneeded = 0)
        {
            return new List<StationData>();
        }
    }
}

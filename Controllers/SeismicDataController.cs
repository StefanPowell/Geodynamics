using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Models;
using EarthQuake.USGS;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeismicDataController : ControllerBase
    {
        API quakeAPI = new API();
        public SeismicDataController()
        {
            
        }

        [HttpGet(Name = "WaveFormData")]
        public async Task<List<miniSEED>> GetWaveFormData()
        {
            List<miniSEED> waveformdata = await quakeAPI.GetWaveFormData();
            return waveformdata;
        }
    }
}

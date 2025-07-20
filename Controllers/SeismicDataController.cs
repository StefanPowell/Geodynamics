using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Models;
using EarthQuake.USGS;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeismicDataController : ControllerBase
    {
        private readonly IUSGSQUAKEAPI _api;
        public SeismicDataController(IUSGSQUAKEAPI api)
        {
            _api = api;
        }

        [HttpGet(Name = "WaveFormData")]
        public async Task<List<miniSEED>> GetWaveFormData()
        {
            List<miniSEED> waveformdata = await _api.GetWaveFormData();
            return waveformdata;
        }
    }
}

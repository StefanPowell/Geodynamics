using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EarthQuake.Persistence.Models;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaveFormController : Controller
    {
        private ILogger<WaveFormController> _logger;
        private readonly IUSGSQUAKEAPI _api;
        private readonly IEarthquakeRepository _earthquakeRepository;

        public WaveFormController(ILogger<WaveFormController> logger, IUSGSQUAKEAPI api, IEarthquakeRepository earthquakeRepository)
        {
            _earthquakeRepository = earthquakeRepository;
            _logger = logger;
            _api = api;
        }

        [HttpPost("InsertWaveformData")]
        public void InsertWaveFormData(ServiceIrisEduData data, DateTime startTime, DateTime endTime)
        {
            try
            {
                _api.PostWaveFormData(startTime, endTime, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        [HttpGet("GetWaveFormData")]
        public void GetWaveFormData(ServiceIrisEduData data, DateTime startTime, DateTime endTime)
        {

        }
    }
}

using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuakeDataController : ControllerBase
    {
        private readonly ILogger<QuakeDataController> _logger;
        private readonly IUSGSQUAKEAPI _api;
        private readonly IEarthquakeRepository _earthquakeRepository;

        public QuakeDataController(ILogger<QuakeDataController> logger, IUSGSQUAKEAPI api, IEarthquakeRepository earthquakeRepository)
        {
            _earthquakeRepository = earthquakeRepository;
            _logger = logger;
            _api = api;
        }

        [HttpPost("today")]
        public void PostTodayQuakes()
        {
            try
            {
                _api.SendQuery(DateTime.Now, DateTime.Now.AddDays(1));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
        }

        [HttpPost("datetimerange")]
        public async Task<int> PostQuakesBetweenDate(DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                return await _api.SendQuery(startDateTime, endDateTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return 0;
            }
        }


        [HttpGet(Name = "GetQuakes")]
        public async Task<ActionResult<List<DBFeature>>> GetQuakeData(int valuesToShow)
        {
            try
            {
               List<DBFeature> allquakes = await _earthquakeRepository.GetLatestQuakes(valuesToShow);
               return Ok(allquakes);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                                    $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
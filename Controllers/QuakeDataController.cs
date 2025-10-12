using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuakeDataController : ControllerBase
    {
        private readonly ILogger<QuakeDataController> _logger;
        private readonly IUSGSQUAKEAPI _api;

        public QuakeDataController(ILogger<QuakeDataController> logger, IUSGSQUAKEAPI api)
        {
            _logger = logger;
            _api = api;
        }

        [HttpPost("today")]
        public void PostTodayQuakes()
        {
            try
            {
                _api.SendQuery(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            
        }

        [HttpPost("daterange")]
        public void PostQuakesBetweenDate(DateTime startdate, DateTime enddate)
        {
            try
            {
                _api.SendQuery(DateOnly.FromDateTime(startdate), DateOnly.FromDateTime(enddate));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        [HttpGet(Name = "GetQuakes")]
        public async Task<ActionResult<List<EarthQuakeView>>> GetQuakeData(int valuesToShow)
        {
            try
            {
                List<Feature> test = await _api.GetQuakesQuery();
                test = test.Take(valuesToShow).ToList();

                List<EarthQuakeView> dataforfrontView = new List<EarthQuakeView>();
                foreach (Feature feature in test)
                {
                    dataforfrontView.Add(new EarthQuakeView(feature));
                }
                return StatusCode(StatusCodes.Status200OK,
                                    dataforfrontView);
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                                    $"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
using EarthQuake.Models;
using EarthQuake.USGS;
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
            _api.SendQuery(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        }

        [HttpPost("daterange")]
        public void PostQuakesBetweenDate(DateTime startdate, DateTime enddate)
        {
            //quakeAPI.SendQuery(DateOnly.FromDateTime(startdate), DateOnly.FromDateTime(enddate));
        }

        [HttpGet(Name = "GetQuakes")]
        public async Task<List<EarthQuakeView>> GetQuakeData()
        {
            //only trying to put 12 on the front screen
            List<Feature> test = await _api.GetQuakesQuery();
            test = test.Take(12).ToList();

            List<EarthQuakeView> dataforfrontView = new List<EarthQuakeView>();
            foreach (Feature feature in test) {
                dataforfrontView.Add(new EarthQuakeView(feature));
            }
            return dataforfrontView;
        }
    }
}
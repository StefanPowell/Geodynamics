using EarthQuake.Models;
using EarthQuake.USGS;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuakeDataController : ControllerBase
    {
        private readonly ILogger<QuakeDataController> _logger;
        API quakeAPI = new API();

        public QuakeDataController(ILogger<QuakeDataController> logger)
        {
            _logger = logger;
        }

        [HttpPost("today")]
        public void PostTodayQuakes()
        {
            quakeAPI.SendQuery(DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        }

        [HttpPost("daterange")]
        public void PostQuakesBetweenDate(DateTime startdate, DateTime enddate)
        {
            quakeAPI.SendQuery(DateOnly.FromDateTime(startdate), DateOnly.FromDateTime(enddate));
        }

        [HttpGet(Name = "GetQuakes")]
        public async Task<List<Feature>> GetQuakeData()
        {
            List<Feature> test = quakeAPI.GetQuakesQuery(DateOnly.FromDateTime(DateTime.Now.AddDays(-7)), DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));
            return test;
        }
    }
}
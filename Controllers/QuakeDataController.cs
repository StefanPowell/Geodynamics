using EarthQuake.Models;
using EarthQuake.USGS;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuakeDataController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<QuakeDataController> _logger;

        public QuakeDataController(ILogger<QuakeDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetQuakes")]
        public List<EarthQuakeFeature> GetQuakeData()
        {
            API x = new API();
            x.SendQuery("", DateTime.Now, DateTime.Now);
            return new List<EarthQuakeFeature>();
        }
    }
}
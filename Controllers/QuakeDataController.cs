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

        public QuakeDataController(ILogger<QuakeDataController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetQuakes")]
        public async Task<List<Feature>> GetQuakeData()
        {
            API x = new API();
            List<Feature> test = await x.SendQuery("", DateOnly.FromDateTime(DateTime.Now.AddDays(-7)), DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));
            return test;
        }
    }
}
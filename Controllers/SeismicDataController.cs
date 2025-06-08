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
        public SeismicDataController()
        {
            
        }

        [HttpGet(Name = "WaveFormData")]
        public miniSEED GetQuakeData(ServiceIrisEduDataSelect data)
        {
            return new miniSEED();
        }
    }
}

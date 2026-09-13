using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EarthQuake.Persistence.Models;
using EarthQuake.Models;

namespace EarthQuake.Controllers;

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


    [HttpGet("GetWaveFormData")]
    public async Task<ActionResult<List<miniSEED>>> GetWaveFormData(DateTime startTime, DateTime endTime, string network, string station, string location = "00", string channel = "BH*", string format = "geocsv.inline")
    {
        try
        {
            List<miniSEED> waveform = await _api.GetWaveFormData(startTime, endTime, network, station, location, channel, format); ;
            return Ok(waveform);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest,
                                    $"An error occurred while processing the request: {ex.Message}");
        }
    }
}

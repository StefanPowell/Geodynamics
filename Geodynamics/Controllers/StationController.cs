using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.USGS.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationController : Controller
{
    private ILogger<WaveFormController> _logger;
    private readonly IUSGSQUAKEAPI _api;
    private readonly IStationRepository _stationRepository;

    public StationController(ILogger<WaveFormController> logger, IUSGSQUAKEAPI api, IStationRepository stationRepository)
    {
        _logger = logger;
        _api = api;
        _stationRepository = stationRepository;
    }

    [HttpGet("GetClosestStations")]
    public async Task<ActionResult<List<StationData>>> GetStation(int stationCount, double latitude, double longtiude, int maxradius)
    {
        try
        {
            List<StationData>? stations = await _api.GetStations(latitude, longtiude, stationCount, maxradius);
            return Ok(stations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(StatusCodes.Status400BadRequest,
                                    $"An error occurred while processing the request: {ex.Message}");
        }
    }
}

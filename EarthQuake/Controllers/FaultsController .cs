using EarthQuake.Models;
using EarthQuake.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EarthQuake.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaultsController : ControllerBase
    {
        private readonly FaultRepository _repo;

        public FaultsController(FaultRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var faults = await _repo.GetAllFaultsAsync();
            return Ok(faults);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Fault fault)
        {
            await _repo.AddFaultAsync(fault);
            return Ok("Inserted");
        }
    }
}

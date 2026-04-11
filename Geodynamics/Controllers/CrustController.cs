using CsvHelper;
using CsvHelper.Configuration;
using EarthQuake.Extensions;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EarthQuake.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CrustController : ControllerBase
{
    private readonly IStressRepository _stressRepository;

    public CrustController(IStressRepository stressRepository)
    {
        _stressRepository = stressRepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> ParseCrustStress(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToUpper(),
            MissingFieldFound = null,
            HeaderValidated = null,
            // FORCE all empty CSV cells to be null for numeric types
        };

        using var csv = new CsvReader(reader, config);

        // REGISTER THE MAP HERE
        csv.Context.RegisterClassMap<CrustStressMap>();

        try
        {
            // Use IEnumerable to process records more efficiently
            var records = csv.GetRecords<CrustStress>().ToList();
            await _stressRepository.SaveCrustStressField(records);
            return Ok(new { count = records.Count });
        }
        catch (Exception ex)
        {
            return BadRequest($"CSV Parsing Error: {ex.Message}");
        }
    }


}

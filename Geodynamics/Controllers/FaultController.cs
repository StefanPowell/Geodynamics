using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EarthQuake.Controllers;

public class FaultController : Controller
{
    private readonly IFaultRepository _faultRepository;

    public FaultController(IFaultRepository faultRepository)
    {
        _faultRepository = faultRepository;
    }


    [HttpPost("upload")]
    public async Task ParseFaults(IFormFile file)
    {
        try
        {
            using var reader = new StreamReader(file.OpenReadStream());
            string json = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            FaultFeatureCollection collection = JsonSerializer.Deserialize<FaultFeatureCollection>(json, options);

            List<Fault> faults = collection.Features.Select(f => new Fault
            {
                Type = f.Type,
                AverageDip = f.Properties.AverageDip,
                AverageRake = f.Properties.AverageRake,
                CatalogId = f.Properties.CatalogId,
                CatalogName = f.Properties.CatalogName,
                DipDir = f.Properties.DipDir,
                LowerSeisDepth = f.Properties.LowerSeisDepth,
                Name = f.Properties.Name,
                NetSlipRate = f.Properties.NetSlipRate,
                SlipType = f.Properties.SlipType,
                UpperSeisDepth = f.Properties.UpperSeisDepth,
                GeometryType = f.Geometry.Type,
                Coordinates = f.Geometry.Coordinates
            }).ToList();

            await _faultRepository.SaveFaultData(faults);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}

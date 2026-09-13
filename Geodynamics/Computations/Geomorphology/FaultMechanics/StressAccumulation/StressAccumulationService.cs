using EarthQuake.Interface.Computations.Geomorphology.FaultMechanics.StressAccumulation;
using EarthQuake.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using System.Text.Json;
using System.Timers;

//stop this service fro now, its killing the processor
namespace EarthQuake.Computations.Geomorphology.FaultMechanics.StressAccumulation
{
    public class StressAccumulationService : IStressAccumulationService
    {
        private readonly ILogger<StressAccumulationService> _logger;
        private readonly string _jsonString;
        private readonly System.Timers.Timer _timer;
        private readonly IEarthquakeRepository _earthquakeRepository;
        private readonly IWebHostEnvironment _env;

        public StressAccumulationService(IEarthquakeRepository earthquakeRepository, ILogger<StressAccumulationService> logger, IWebHostEnvironment env)
        {
            _earthquakeRepository = earthquakeRepository;
            _env = env;

            string filePath = Path.Combine(
            _env.ContentRootPath,
            "Computations\\Geomorphology\\FaultLines",
            "gem_active_faults.geojson");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"GeoJSON file not found at path: {filePath}");

            _jsonString = File.ReadAllText(filePath);

            // Run once every 24 hours (in milliseconds)
            _timer = new(1000)
            {
                AutoReset = false
            };

            _timer.Elapsed += AccumulatedStressOnFault;
            _logger = logger;
            _env = env;
        }

        public void Start()
        {
            _timer.Start(); 
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void AccumulatedStressOnFault(object? sender, ElapsedEventArgs e)
        {
            try
            {
                CalculateAccumulatedStress().ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error calculating stress accumulation: {ex.Message}");
            }
            finally
            {
                //_timer.Start();
            }
        }

        public async Task CalculateAccumulatedStress()
        {
            try
            {
                List<Fault> allFaults = GetFaultLines(_jsonString);
                foreach (Fault fault in allFaults)
                {
                    FaultStress faultStress = ComputeStressAccumulation(fault);
                    await _earthquakeRepository.UpdateFaultStress(faultStress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error calculating stress accumulation: {ex.Message}");
            }
        }

        public List<Fault> GetFaultLines(string jsonString)
        {
            var faultList = new List<Fault>();

            using var document = JsonDocument.Parse(jsonString);
            var root = document.RootElement;

            if (root.TryGetProperty("features", out JsonElement features))
            {
                foreach (var feature in features.EnumerateArray())
                {
                    var properties = feature.GetProperty("properties");
                    var geometry = feature.GetProperty("geometry");

                    string SafeGet(JsonElement obj, string propertyName)
                    {
                        if (obj.TryGetProperty(propertyName, out var prop))
                        {
                            var value = prop.GetString();
                            // Normalize and handle "nan" or empty values
                            if (string.IsNullOrWhiteSpace(value) ||
                                value.Contains("nan", StringComparison.OrdinalIgnoreCase))
                            {
                                return "NaN";
                            }
                            return value;
                        }
                        return "NaN";
                    }

                    var fault = new Fault
                    {
                        Type = feature.GetProperty("type").GetString() ?? "Feature",
                        AverageDip = SafeGet(properties, "average_dip"),
                        AverageRake = SafeGet(properties, "average_rake"),
                        CatalogId = SafeGet(properties, "catalog_id"),
                        CatalogName = SafeGet(properties, "catalog_name"),
                        DipDir = SafeGet(properties, "dip_dir"),
                        LowerSeisDepth = SafeGet(properties, "lower_seis_depth"),
                        Name = SafeGet(properties, "name"),
                        NetSlipRate = SafeGet(properties, "net_slip_rate"),
                        SlipType = SafeGet(properties, "slip_type"),
                        UpperSeisDepth = SafeGet(properties, "upper_seis_depth"),
                        GeometryType = geometry.GetProperty("type").GetString(),
                        Coordinates = new List<List<double>>()
                    };

                    // Parse coordinates safely
                    foreach (var coordArray in geometry.GetProperty("coordinates").EnumerateArray())
                    {
                        var point = new List<double>();
                        foreach (var num in coordArray.EnumerateArray())
                        {
                            point.Add(num.GetDouble());
                        }
                        fault.Coordinates.Add(point);
                        _logger.LogInformation($"Loaded fault {fault.CatalogId}");
                    }

                    faultList.Add(fault);
                }
            }

            return faultList;
        }


        public FaultStress ComputeStressAccumulation(Fault fault, double years = 100.0, double shearModulusPa = 30e9)
        {
            //I need a service that get the uodated net slip rate and then do the calculation from that

            // 1. Parse net slip rate (mm/yr -> m/yr)
            double slipRate = 0.0;
            if (!string.IsNullOrEmpty(fault.NetSlipRate))
            {
                var cleaned = fault.NetSlipRate.Trim('(', ')');
                var parts = cleaned.Split(',');
                if (parts.Length > 0 && double.TryParse(parts[0], out double avgRate))
                {
                    slipRate = avgRate / 1000.0; // m/yr
                }
            }

            // 2. Compute locked thickness D (m)
            double D = 10000.0; // default 10 km
            if (double.TryParse(fault.UpperSeisDepth?.Split(',')[0], out double upper) &&
                double.TryParse(fault.LowerSeisDepth?.Split(',')[0], out double lower))
            {
                D = (lower - upper) * 1000.0; // km -> m
            }

            // 3. Compute rupture length L (m) using haversine formula
            double L = 0.0;
            if (fault.Coordinates != null && fault.Coordinates.Count > 1)
            {
                double R = 6371000.0; // Earth radius in meters
                for (int i = 0; i < fault.Coordinates.Count - 1; i++)
                {
                    double lat1 = fault.Coordinates[i][1] * Math.PI / 180.0;
                    double lon1 = fault.Coordinates[i][0] * Math.PI / 180.0;
                    double lat2 = fault.Coordinates[i + 1][1] * Math.PI / 180.0;
                    double lon2 = fault.Coordinates[i + 1][0] * Math.PI / 180.0;

                    double dLat = lat2 - lat1;
                    double dLon = lon2 - lon1;
                    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                               Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    L += R * c;
                }
            }

            // 4. Compute shear stress rate (Pa/yr) and in MPa/yr
            double tauDot = shearModulusPa * slipRate / D;      // Pa/yr
            double tauDotMPa = tauDot / 1e6;

            // 5. Accumulated stress over given years
            double deltaTau = tauDot * years;                  // Pa
            double deltaTauMPa = deltaTau / 1e6;              // MPa

            // 6. Approximate slip if stress released over rupture length
            double expectedSlip = (deltaTau * L) / shearModulusPa; // meters

            // 7. Seismic moment and moment magnitude
            double area = L * D;          // m^2
            double M0 = shearModulusPa * area * expectedSlip; // N·m
            double Mw = (2.0 / 3.0) * (Math.Log10(M0) - 9.1);

            // 8. Output results
            // Theses results should go into the datbase
            Console.WriteLine($"Fault: {fault.Name}");
            Console.WriteLine($"Slip Rate (m/yr): {slipRate:F6}");
            Console.WriteLine($"Locked Thickness (m): {D:F1}");
            Console.WriteLine($"Rupture Length (m): {L:F1}");
            Console.WriteLine($"Shear Stress Rate (MPa/yr): {tauDotMPa:F6}");
            Console.WriteLine($"Accumulated Stress over {years} yrs (MPa): {deltaTauMPa:F3}");
            Console.WriteLine($"Expected Slip (m): {expectedSlip:F3}");
            Console.WriteLine($"Moment Magnitude Mw: {Mw:F2}");

            FaultStress faultStress = new()
            {
                FaultName = fault.Name,
                SlipRate_m_per_yr = slipRate,
                LockedThickness_m = D,
                RuptureLength_m = L,
                ShearStressRate_MPa_per_yr = tauDotMPa,
                AccumulatedStress_MPa = deltaTauMPa,
                ExpectedSlip_m = expectedSlip,
                MomentMagnitude_Mw = Mw,
            };

            return faultStress;
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}


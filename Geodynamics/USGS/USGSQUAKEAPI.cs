using EarthQuake.Models;
using Newtonsoft.Json;
using System.Globalization;
using EarthQuake.USGS.Interfaces;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;

namespace EarthQuake.USGS
{
    public class USGSQUAKEAPI : IUSGSQUAKEAPI
    {
        private readonly ILogger<USGSQUAKEAPI> _logger;
        private readonly IEarthquakeRepository earthquakeRepo;
        private readonly HttpClient _client;

        public USGSQUAKEAPI(IEarthquakeRepository repo, HttpClient client, ILogger<USGSQUAKEAPI> logger)
        {
            earthquakeRepo = repo ?? throw new ArgumentNullException(nameof(repo));
            _client = client;
            _logger = logger;
        }

        public async Task<int> SendQuery(DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                string url = $"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={startDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss")}&endtime={endDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss")}";
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Root? EarthQuakeDataRoot = JsonConvert.DeserializeObject<Root>(responseBody);
                if (EarthQuakeDataRoot != null)
                {
                    List<Feature> featurelist = EarthQuakeDataRoot.features;
                    if(featurelist.Count() > 0)
                    {
                        await earthquakeRepo.SaveData(featurelist);
                    }
                    return featurelist.Count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return 0;
            }
        }

        public async Task <List<Feature>> GetQuakesQuery()
        {
            string url = "https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=2026-03-11T00:00:00&endtime=2026-03-11T00:59:00";
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Root EarthQuakeDataRoot = JsonConvert.DeserializeObject<Root>(responseBody);
            List<Feature> featurelist = EarthQuakeDataRoot.features;
            return featurelist;            
        }

        public async Task<List<StationData>> GetStations(double latitude, double longitude, int totalstations, int maxradius)
        {          
            string url = $"https://service.iris.edu/fdsnws/station/1/query?lat={latitude}&lon={longitude}&maxradius={maxradius}&level=station&format=text";
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var stationList = new List<StationData>();

            string[] lines = responseBody.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts.Length < 8) continue;

                stationList.Add(new StationData
                {
                    network = parts[0],
                    station = parts[1],
                    latitude = double.Parse(parts[2], CultureInfo.InvariantCulture),
                    longitude = double.Parse(parts[3], CultureInfo.InvariantCulture),
                    elevation = double.Parse(parts[4], CultureInfo.InvariantCulture),
                    sitename = parts[5],
                    starttime = parts[6],
                    endtime = parts[7]
                });
            }
            return stationList;
        }

        public async Task PostWaveFormData(DateTime startTime, DateTime endTime, ServiceIrisEduData data = null)
        {
            string url = $"https://service.iris.edu/fdsnws/dataselect/1/query?net=IU&sta=ANMO&loc=00&cha=BHZ&starttime={startTime}&endtime={endTime}&format=geocsv.inline";
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            List<miniSEED> WaveFormData = new List<miniSEED>();

            string[] lines = responseBody.Split('\n');

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("Time"))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length != 2)
                    continue;

                if (DateTime.TryParse(parts[0], null, DateTimeStyles.AdjustToUniversal, out DateTime time) &&
                    double.TryParse(parts[1], out double amplitude))
                {
                    WaveFormData.Add(new miniSEED
                    {
                        time = time,
                        amplitude = amplitude
                    });
                }
            }
            await earthquakeRepo.SaveWaveFormData(WaveFormData);
        }   
    }
}

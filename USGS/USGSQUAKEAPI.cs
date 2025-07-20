using EarthQuake.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using EarthQuake.Repository;
using EarthQuake.Computations.Seismology.Models;
using EarthQuake.USGS.Interfaces;
using EarthQuake.Interface;

namespace EarthQuake.USGS
{
    //run api daily
    //on api daily run save all earthquake data in database in AWS cloud
    public class USGSQUAKEAPI : IUSGSQUAKEAPI
    {
        private IApplicationRepoContext _repo;

        public USGSQUAKEAPI(IApplicationRepoContext repo)
        {
           _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }



        public async void SendQuery(DateOnly starttime, DateOnly endtime)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=" + starttime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) +"&endtime=" + endtime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Root EarthQuakeDataRoot = JsonConvert.DeserializeObject<Root>(responseBody);
                    List<Feature> featurelist = EarthQuakeDataRoot.features;
                    _repo.SaveData(featurelist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //I need a logger, curently will use internal logger -- move to some other logger later
                //Build Error Catcher to populate all errors
            }
            
        }

        public async Task <List<Feature>> GetQuakesQuery()
        {
            //get all the data in a date range and return teh values
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=2025-06-13&endtime=2025-06-14";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Root EarthQuakeDataRoot = JsonConvert.DeserializeObject<Root>(responseBody);
                    List<Feature> featurelist = EarthQuakeDataRoot.features;
                    return featurelist;
                }
        }

        public async Task<List<StationData>> GetStations(double latitude, double longitude, int totalstations, int maxradius)
        {
            //use double latitude, double longitude
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://service.iris.edu/fdsnws/station/1/query?lat={latitude}&lon={longitude}&maxradius={maxradius}&level=station&format=text";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var stationList = new List<StationData>();

                string[] lines = responseBody.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                // Skip the header
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
        }

        public async Task<List<miniSEED>> GetWaveFormData(ServiceIrisEduData data = null)
        {
            //use double latitude, double longitude
            using (HttpClient client = new HttpClient())
            {
                string url = "https://service.iris.edu/fdsnws/dataselect/1/query?net=IU&sta=ANMO&loc=00&cha=BHZ&starttime=2010-02-27T06:30:00&endtime=2010-02-27T06:45:00&format=geocsv.inline";
                HttpResponseMessage response = await client.GetAsync(url);
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
                return WaveFormData;
            }
        }   
    }
}

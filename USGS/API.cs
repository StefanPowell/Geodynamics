using EarthQuake.Models;
using System.Text.Json;

namespace EarthQuake.USGS
{
    //run api daily
    //on api daily run save all earthquake data in database in AWS cloud
    public class API
    {
        public async void SendQuery(string format, DateTime starttime, DateTime endtime)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=2014-01-01&endtime=2014-01-02");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            QueryData? earthQuakeQueryData = JsonSerializer.Deserialize<QueryData>(responseBody); 

            Console.WriteLine(responseBody);
            Console.ReadLine();
        }

        public List<EarthQuakeFeature> GetQuakes(DateTime starttime, DateTime endtime)
        {
            return new List<EarthQuakeFeature> { new EarthQuakeFeature() };
        }
    }
}

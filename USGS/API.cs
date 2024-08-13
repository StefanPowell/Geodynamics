using EarthQuake.Models;
using System.Text.Json;

namespace EarthQuake.USGS
{
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
    }
}

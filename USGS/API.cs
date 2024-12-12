using EarthQuake.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;
using System.Globalization;

namespace EarthQuake.USGS
{
    //run api daily
    //on api daily run save all earthquake data in database in AWS cloud
    public class API
    {
        public async Task<List<Feature>> SendQuery(string format, DateOnly starttime, DateOnly endtime)
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
                    return featurelist;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Feature>();
                //Build Error Catcher to populate all errors
            }
            
        }

        public List<EarthQuakeFeature> GetQuakes(DateTime starttime, DateTime endtime)
        {
            return new List<EarthQuakeFeature> { new EarthQuakeFeature() };
        }
    }
}

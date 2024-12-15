using EarthQuake.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using EarthQuake.Repository;

namespace EarthQuake.USGS
{
    //run api daily
    //on api daily run save all earthquake data in database in AWS cloud
    public class API
    {
        private List<Feature> _repodatabase;
        private ApplicationRepoContext _context;

        public API()
        {
            _repodatabase = new List<Feature>();  
            _context = new ApplicationRepoContext(_repodatabase);  
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
                    _context.SaveData(featurelist);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Build Error Catcher to populate all errors
            }
            
        }

        public List<EarthQuakeFeature> GetQuakes(DateTime starttime, DateTime endtime)
        {
            return new List<EarthQuakeFeature> { new EarthQuakeFeature() };
        }
    }
}

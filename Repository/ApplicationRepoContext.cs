using EarthQuake.Models;
using EarthQuake.Computations.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EarthQuake.Repository
{
    public class ApplicationRepoContext
    {
        private List<Feature> _repodatabase;

        public ApplicationRepoContext(List<Feature> repodatabase)
        {
            repodatabase = repodatabase ?? throw new ArgumentNullException(nameof(repodatabase));
        }

        public void SaveData(List<Feature> data) { 
            _repodatabase.AddRange(data);
        }

        public void DeleteData(Feature datapoint) {
            _repodatabase.Remove(datapoint);
        }

        public List<Feature> GetBetweenDates(DateTime startdate, DateTime enddate)
        {
           
           long startdateEpoch = convertToEpoch(startdate);
            // _repodatabase.Select(x => x.properties.);
            return new List<Feature>();
        }

        public List<Feature> GetRepodatabase() {  
            return _repodatabase; 
        }
    }
}

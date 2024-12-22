using EarthQuake.Models;

namespace EarthQuake.Interface
{
    public interface IApplicationRepoContext
    {
        public void SaveData(List<Feature> data);
        public void DeleteData(Feature datapoint);
        public List<Feature> GetBetweenDates(DateOnly startdate, DateOnly enddate);
        public List<Feature> GetRepodatabase();
    }
}

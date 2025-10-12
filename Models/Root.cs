using EarthQuake.Persistence.Models;

namespace EarthQuake.Models;

 public class Root
    {
        public string type { get; set; }
        public MetaData metadata { get; set; }
        public List<Feature> features { get; set; }
        public List<double> bbox { get; set; }
    }

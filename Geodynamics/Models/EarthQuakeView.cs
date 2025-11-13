using EarthQuake.Persistence.Models;

namespace EarthQuake.Models
{
    public class EarthQuakeView
    {
        public double mag { get; set; }
        public double depth { get; set; }
        public string place { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public double time { get; set; }

        public EarthQuakeView(Feature quake) {
            mag = quake.properties.mag;
            depth = quake.geometry.coordinates[2];
            place = quake.properties.place;
            lat = quake.geometry.coordinates[0];
            lon = quake.geometry.coordinates[1];
            time = quake.properties.time;
        }
    }
}

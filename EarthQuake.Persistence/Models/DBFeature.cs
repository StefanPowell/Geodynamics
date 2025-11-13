

namespace EarthQuake.Persistence.Models;

public class DBFeature
{
    public string Id { get; set; }
    public string FeatureType { get; set; }
    public string GeometryType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Depth { get; set; }
    public double Magnitude { get; set; }
    public string Place { get; set; }
    public DateTime QuakeDateTime { get; set; }
}

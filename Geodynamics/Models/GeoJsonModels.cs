using System.Text.Json.Serialization;

namespace EarthQuake.Models
{
    public class FaultFeature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Feature";

        [JsonPropertyName("properties")]
        public FaultProperties Properties { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }

    public class FaultProperties
    {
        [JsonPropertyName("average_dip")]
        public string AverageDip { get; set; }  // e.g. "(38,,)"

        [JsonPropertyName("average_rake")]
        public string AverageRake { get; set; }  // e.g. "(90.0,,)"

        [JsonPropertyName("catalog_id")]
        public string CatalogId { get; set; }  // e.g. "UCF_2"

        [JsonPropertyName("catalog_name")]
        public string CatalogName { get; set; }  // e.g. "UCERF3"

        [JsonPropertyName("dip_dir")]
        public string DipDir { get; set; }  // e.g. "E"

        [JsonPropertyName("lower_seis_depth")]
        public string LowerSeisDepth { get; set; }  // e.g. "(16.0,,)"

        [JsonPropertyName("name")]
        public string Name { get; set; }  // e.g. "Mount Diablo Thrust"

        [JsonPropertyName("net_slip_rate")]
        public string NetSlipRate { get; set; }  // e.g. "(1.55,0.8,2.22)"

        [JsonPropertyName("slip_type")]
        public string SlipType { get; set; }  // e.g. "Reverse"

        [JsonPropertyName("upper_seis_depth")]
        public string UpperSeisDepth { get; set; }  // e.g. "(8.0,,)"
    }

    public class Geometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "LineString";

        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; }  // List of [longitude, latitude] pairs
    }

}

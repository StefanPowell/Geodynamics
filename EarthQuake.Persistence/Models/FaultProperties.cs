using System.Text.Json.Serialization;

namespace EarthQuake.Persistence.Models;

public class FaultProperties
{
    [JsonPropertyName("average_dip")]
    public string AverageDip { get; set; }

    [JsonPropertyName("average_rake")]
    public string AverageRake { get; set; }

    [JsonPropertyName("catalog_id")]
    public string CatalogId { get; set; }

    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    [JsonPropertyName("dip_dir")]
    public string DipDir { get; set; }

    [JsonPropertyName("lower_seis_depth")]
    public string LowerSeisDepth { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("net_slip_rate")]
    public string NetSlipRate { get; set; }

    [JsonPropertyName("slip_type")]
    public string SlipType { get; set; }

    [JsonPropertyName("upper_seis_depth")]
    public string UpperSeisDepth { get; set; }
}

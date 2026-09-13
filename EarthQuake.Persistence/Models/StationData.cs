using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class StationData
{
    [JsonPropertyName("network")]
    public string network { get; set; }

    [JsonPropertyName("station")]
    public string station { get; set; }

    [JsonPropertyName("latitude")]
    public double latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double longitude { get; set; }
    [JsonPropertyName("elevation")]
    public double elevation { get; set; }
    [JsonPropertyName("sitename")]
    public string sitename { get; set; }
    [JsonPropertyName("starttime")]
    public string starttime { get; set; }
    [JsonPropertyName("endtime")]
    public string endtime { get; set; }
}

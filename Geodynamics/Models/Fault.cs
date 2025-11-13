using System.Text.Json.Serialization;

namespace EarthQuake.Models
{
    public class Fault
    {
        public string Type { get; set; } = "Feature";
        public string AverageDip { get; set; } 
        public string AverageRake { get; set; } 
        public string CatalogId { get; set; } 
        public string CatalogName { get; set; }  
        public string DipDir { get; set; } 
        public string LowerSeisDepth { get; set; } 
        public string Name { get; set; } 
        public string NetSlipRate { get; set; }  
        public string SlipType { get; set; }
        public string UpperSeisDepth { get; set; } 
        public string GeometryType { get; set; } = "LineString";
        public List<List<double>> Coordinates { get; set; }
    }
}

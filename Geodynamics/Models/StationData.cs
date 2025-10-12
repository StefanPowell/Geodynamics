namespace EarthQuake.Models
{
    public class StationData
    {
        public string network {get; set;}
        public string station { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double elevation { get; set; }
        public string sitename { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
    }
}

namespace EarthQuake.Models
{
    public class QueryData
    {
        public string type { get; set; }
        public JsonContent metadata { get; set; }
        public List<JsonContent> features { get; set; }
    }
}

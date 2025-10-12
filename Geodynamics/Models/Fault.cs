namespace EarthQuake.Models
{
    public class Fault
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Dip { get; set; }
        public DateTime LastMovement { get; set; }
    }
}

namespace EarthQuake.Computations.Seismology.Models
{
    public class ServiceIrisEduData
    {
        public string network {  get; set; }
        public string station { get; set; }
        public string location { get; set; }
        public string channel { get; set; }
        public TimeData timedata { get; set; }
    }

    public class TimeData{
        public DateTime starttime {  get; set; }
        public DateTime endtime { get; set; }
    }

    public class miniSEED{
        public DateTime time {  get; set; }
        public double amplitude { get; set; }
    }
}

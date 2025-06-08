namespace EarthQuake.Computations.Seismology.Models
{
    public class ServiceIrisEduDataSelect
    {
        public string network;
        public string station;
        public string location;
        public string channel;
        public TimeData timedata;
    }

    public class TimeData{
        public DateTime starttime;
        public DateTime endtime;
    }

    public class miniSEED{
        public DateTime time;
        public double amplitude;
    }
}

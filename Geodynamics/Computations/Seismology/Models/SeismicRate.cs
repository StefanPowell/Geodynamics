using EarthQuake.Enum;
using EarthQuake.Models;

namespace EarthQuake.Computations.Seismology.Models;

public class SeismicRate
{
    public GeographicalBoundary geographicalBoundary {  get; set; }
    public DateOnly startdate { get; set; }
    public DateOnly enddate { get; set; }
    public TimePeriod timePeriod { get; set; }
    public int timePeriodCount { get; set; }
}

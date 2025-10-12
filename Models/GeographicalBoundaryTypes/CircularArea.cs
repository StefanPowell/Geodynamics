using EarthQuake.Enum;
using EarthQuake.Persistence.Enum;
using EarthQuake.Persistence.Models;

namespace EarthQuake.Models.GeographicalBoundaryTypes
{
    public class CircularArea
    {
        public Coordinate Coordinate { get; set; }
        public float radius { get; set; }
        public UnitOfMeasure unit {  get; set; }
    }
}

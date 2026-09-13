using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class Geometry
{
    public string type { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Depth { get; set; }

    public List<double> coordinates =>
        new List<double>
        {
            Longitude,
            Latitude,
            Depth
        };
}
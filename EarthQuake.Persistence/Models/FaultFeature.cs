using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class FaultFeature
{
    public string Type { get; set; }
    public FaultProperties Properties { get; set; }
    public FaultGeometry Geometry { get; set; }
}

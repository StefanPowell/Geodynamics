using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Models;

public class FaultFeatureCollection
{
    public string Type { get; set; }
    public List<FaultFeature> Features { get; set; }
}

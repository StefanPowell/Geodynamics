using EarthQuake.Interface.Computations.Geomorphology.FaultMechanics.NetSlipRate;
using EarthQuake.Models;

namespace EarthQuake.Computations.Geomorphology.FaultMechanics.NetSlipRate;

public class NetSlipRateService : INetSlipRateService
{
    public NetSlipRateService()
    {

    }

    public double GeodeticSlipRate(Fault fault)
    {
        double x = 0.0;
        return x;
    }

    public static double ComputeFaultStrike(Fault fault)
    {
        var start = fault.Coordinates.First();
        var end = fault.Coordinates.Last();

        double lon1 = start[0], lat1 = start[1];
        double lon2 = end[0], lat2 = end[1];

        // Convert to radians
        double dLon = (lon2 - lon1) * Math.PI / 180.0;
        double y = Math.Sin(dLon) * Math.Cos(lat2 * Math.PI / 180.0);
        double x = Math.Cos(lat1 * Math.PI / 180.0) * Math.Sin(lat2 * Math.PI / 180.0)
                   - Math.Sin(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) * Math.Cos(dLon);

        double strike = Math.Atan2(y, x) * 180.0 / Math.PI;
        if (strike < 0) strike += 360.0;

        return strike; // in degrees from north
    }
}

using EarthQuake.Computations.Geomorphology.FaultMechanics.NetSlipRate;
using EarthQuake.Computations.Geomorphology.FaultMechanics.StressAccumulation;
using EarthQuake.Interface;
using EarthQuake.Interface.Computations.Geomorphology.FaultMechanics.NetSlipRate;
using EarthQuake.Interface.Computations.Geomorphology.FaultMechanics.StressAccumulation;

namespace EarthQuake.Computations.Geomorphology.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGeomorphologyServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddTransient<IStressAccumulationService, StressAccumulationService>()
                       .AddTransient<INetSlipRateService, NetSlipRateService>();
    }
}

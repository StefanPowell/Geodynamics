using EarthQuake.Computations.Seismology.Extensions;
using EarthQuake.Computations.Seismology.SeismicRateChange;
using EarthQuake.Interface;

namespace EarthQuake.Computations;

public static class DependencyInjection
{
    public static IServiceCollection AddComputationServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddSeismologyServices(config);
    }
}

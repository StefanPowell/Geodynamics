using EarthQuake.Computations.Seismology.SeismicRateChange;
using EarthQuake.Interface;

namespace EarthQuake.Computations.Seismology.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSeismologyServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddTransient<ISeismicRateChangeService, SeismicRateChangeService>();
    }
}

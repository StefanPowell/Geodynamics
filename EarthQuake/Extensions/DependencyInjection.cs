using EarthQuake.Computations;
using EarthQuake.Computations.Seismology.SeismicRateChange;
using EarthQuake.Interface;
using EarthQuake.Persistence.Extensions;
using EarthQuake.Predictablity;
using EarthQuake.Repository;

namespace EarthQuake.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEarthQuakeServices(this IServiceCollection services, IConfiguration config)
        {
            return services.EarthQuakePersistentLayer(config)
                           .AddSingleton<IFaultRepository, FaultRepository>()
                           .AddSingleton<IFutureQuakeSimulator, FutureQuakeSimulator>()
                           .AddComputationServices(config);
        }
    }
}

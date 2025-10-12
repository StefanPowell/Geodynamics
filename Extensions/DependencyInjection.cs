using EarthQuake.Interface;
using EarthQuake.Persistence.Extensions;
using EarthQuake.Repository;

namespace EarthQuake.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEarthQuakeServices(this IServiceCollection services, IConfiguration config)
        {
            return services.AddSingleton<IFaultRepository, FaultRepository>()
                           .EarthQuakePersistentLayer(config);
        }
    }
}

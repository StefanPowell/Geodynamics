using EarthQuake.Computations;
using EarthQuake.Persistence.Extensions;

namespace EarthQuake.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEarthQuakeServices(this IServiceCollection services, IConfiguration config)
        {
            return services.EarthQuakePersistentLayer(config);
        }
    }
}

using EarthQuake.Persistence.Repository;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EarthQuake.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection EarthQuakePersistentLayer(this IServiceCollection services, IConfiguration config)
    {
        string earthquakeDatabaseConnectionString = config.GetConnectionString("DefaultConnection");

        return services.AddTransient<IEarthquakeRepository, EarthquakeRepository>(svc => new EarthquakeRepository(earthquakeDatabaseConnectionString));
    }
}

using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace EarthQuake.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection EarthQuakePersistentLayer(this IServiceCollection services, IConfiguration config)
    {
        string earthquakeDatabaseConnectionString = config.GetConnectionString("DefaultConnection");

        services.AddTransient<IEarthquakeRepository>(sp =>
            new EarthquakeRepository(earthquakeDatabaseConnectionString));

        services.AddTransient<IDataQueryRepository>(sp =>
            new DataQueryRepository(earthquakeDatabaseConnectionString));

        services.AddTransient<IFaultRepository>(sp =>
            new FaultRepository(earthquakeDatabaseConnectionString));

        services.AddTransient<IStressRepository>(sp =>
            new StressRepository(earthquakeDatabaseConnectionString));

        return services;
    }

}

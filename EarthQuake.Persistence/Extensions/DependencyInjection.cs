using EarthQuake.Persistence.Repository.Abstractions;
using EarthQuake.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EarthQuake.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection EarthQuakePersistentLayer(this IServiceCollection services, IConfiguration config)
    {
        string earthquakeDatabaseConnectionString = config.GetConnectionString("DefaultConnection");

        return services.AddTransient<IEarthquakeRepository, EarthquakeRepository>(svc => new EarthquakeRepository(earthquakeDatabaseConnectionString))
                       .AddTransient<IDataQueryRepository, DataQueryRepository>(svc => new DataQueryRepository(earthquakeDatabaseConnectionString));
    }
}

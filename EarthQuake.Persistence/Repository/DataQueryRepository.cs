using Dapper;
using EarthQuake.Persistence.Enum;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EarthQuake.Persistence.Repository
{
    public class DataQueryRepository : IDataQueryRepository
    {
        private readonly string _connectionString;

        public DataQueryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            SqlConnection connection = new(_connectionString);
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<DBFeature>> GetNumberofEarthquakeEventsInGridArea(double latitudeStart, double latitudeEnd, double longitudeStart, double longitudeEnd, DateOnly startDate, DateOnly endDate)
        {
            using (var sql = CreateConnection())
            {
                var parameters = new
                {
                    latitudeStart = latitudeStart,
                    latitudeEnd = latitudeEnd,
                    longitudeStart = longitudeStart,
                    longitudeEnd = longitudeEnd,
                    startDate = startDate,
                    endDate = endDate,
                };

                IEnumerable<DBFeature> allQuakeEventsInGrid = await sql.QueryAsync<DBFeature>(
                    "dbo.sp_Feature_Get_AllWithGridAndTime",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return allQuakeEventsInGrid;
            }
        }

        public async Task<IEnumerable<Feature>> GetNumberofEarthquakeEventsInCircularArea(double latitude, double longitude, double radius, UnitOfMeasure unit, DateOnly startdate, DateOnly enddate)
        {
            using (var sql = CreateConnection())
            {
                var parameters = new
                {
                    latitude = latitude,
                    longitude = longitude,
                    radius = UseStarndardMeasurement(radius, unit),
                    startDate = startdate,
                    endDate = enddate,
                };

                IEnumerable<Feature> allQuakeEventsInArea = await sql.QueryAsync<Feature>(
                    "dbo.sp_Feature_Get_AllWithinCircularArea",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return allQuakeEventsInArea;
            }
        }

        public double UseStarndardMeasurement(double radius, UnitOfMeasure unit)
        {
            //default is km
            if (unit == UnitOfMeasure.Kilometres)
            {
                return radius;
            }
            else
            {
                throw new ArgumentException("Invalid Unit of Measure Type specified.", nameof(UnitOfMeasure));
            }
        }
    }
}

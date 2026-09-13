using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Data.SqlClient;

namespace EarthQuake.Persistence.Repository;

public class StationRepository : IStationRepository
{
    private readonly string _connectionString;

    public StationRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        SqlConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }
}

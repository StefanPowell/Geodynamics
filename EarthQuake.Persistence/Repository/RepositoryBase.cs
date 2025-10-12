using Microsoft.Data.SqlClient;

namespace EarthQuake.Persistence.Repository;

public class RepositoryBase
{
    private readonly string _connectionString;

    public RepositoryBase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetOpenConnection()
    {
        SqlConnection connection = new(_connectionString);
        connection.Open();

        return connection;
    }
}

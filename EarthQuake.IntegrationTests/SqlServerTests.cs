using System.Data;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;
using Xunit;

namespace EarthQuake.IntegrationTests;

public class SqlServerTests : IAsyncLifetime
{
    private readonly MsSqlContainer _db;

    public SqlServerTests()
    {
        _db = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Your_password123")     
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _db.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _db.StopAsync();
    }

    [Fact]
    public async Task CanConnectToSqlServer()
    {
        using var conn = new SqlConnection(_db.GetConnectionString());
        await conn.OpenAsync();

        Assert.Equal(ConnectionState.Open, conn.State);
    }
}

using EarthQuake.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using EarthQuake.Interface;

namespace EarthQuake.Repository
{
    public class FaultRepository : IFaultRepository
    {
        private readonly string _connectionString;

        public FaultRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Fault>> GetAllFaultsAsync()
        {
            const string sql = "SELECT * FROM faults";
            using var connection = CreateConnection();
            return await connection.QueryAsync<Fault>(sql);
        }

        public async Task<int> AddFaultAsync(Fault fault)
        {
            const string sql = @"
                INSERT INTO faults (name, type, dip, last_movement)
                VALUES (@Name, @Type, @Dip, @LastMovement)";

            using var connection = CreateConnection();
            return await connection.ExecuteAsync(sql, fault);
        }
    }
}

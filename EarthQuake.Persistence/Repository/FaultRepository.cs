using Dapper;
using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EarthQuake.Persistence.Repository;

public class FaultRepository : IFaultRepository
{
    private readonly string _connectionString;

    public FaultRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        SqlConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    public async Task<Fault> GetNearestFaultToCoordinates(double latitude, double longitude)
    {
        using (var sql = CreateConnection())
        {
            var parameters = new
            {
                latitude = latitude,
                longitude = longitude
            };

            Fault closestFaultToEarthquake = await sql.QueryFirstAsync<Fault>(
                "dbo.sp_Fault_Get_ClosestToCoordinates",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return closestFaultToEarthquake;
        }
    }

    public async Task SaveFaultData(List<Fault> faults)
    {
        using (var sql = CreateConnection())
        {
            DataTable? table = new DataTable();

            table.Columns.Add("averageDip", typeof(string));
            table.Columns.Add("averageRake", typeof(string));
            table.Columns.Add("catalogId", typeof(int));
            table.Columns.Add("catalogName", typeof(string));
            table.Columns.Add("dipDir", typeof(string));
            table.Columns.Add("lowerSeisDepth", typeof(string));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("netSlipRate", typeof(string));
            table.Columns.Add("slipType", typeof(string));
            table.Columns.Add("upperSeisDepth", typeof(string));
            table.Columns.Add("geometryType", typeof(string));
            table.Columns.Add("co_ordinateList", typeof(string));

            foreach (var fault in faults)
            {
                string coordinates = string.Join(", ",
                    fault.Coordinates.Select((c, i) =>
                        $"({i + 1}, {c[1]}, {c[0]})")); // (index, lat, lon)

                table.Rows.Add(
                    fault.AverageDip,
                    fault.AverageRake,
                    int.TryParse(fault.CatalogId, out var id) ? id : (int?)null,
                    fault.CatalogName, 
                    fault.DipDir,
                    fault.LowerSeisDepth,
                    fault.Name,
                    fault.NetSlipRate,
                    fault.SlipType,
                    fault.UpperSeisDepth,
                    fault.GeometryType,
                    coordinates
                );
            }

            DynamicParameters? parameters = new DynamicParameters();
            parameters.Add(
                "@FaultLine",
                table.AsTableValuedParameter("dbo.FaultLine")
            );

            await sql.ExecuteAsync(
                "dbo.InsertFaultLines",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }

    public async Task RemoveFaults()
    {

    }

    public async Task UpdateFault(Fault fault)
    {

    }
}

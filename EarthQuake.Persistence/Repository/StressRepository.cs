using Dapper;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EarthQuake.Persistence.Repository;

public class StressRepository : IStressRepository
{
    private readonly string _connectionString;

    public StressRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        SqlConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    public async Task SaveCrustStressField(List<CrustStress> crustStressList)
    {
        using (var sql = CreateConnection())
        {
            DataTable? crustStressTable = new DataTable();

            crustStressTable.Columns.Add("ID", typeof(string));
            crustStressTable.Columns.Add("ISC_ID", typeof(string));
            crustStressTable.Columns.Add("SITE", typeof(string));
            crustStressTable.Columns.Add("LAT", typeof(decimal));
            crustStressTable.Columns.Add("LON", typeof(decimal));
            crustStressTable.Columns.Add("AZI", typeof(decimal));
            crustStressTable.Columns.Add("TYPE", typeof(string));
            crustStressTable.Columns.Add("DEPTH", typeof(decimal));
            crustStressTable.Columns.Add("QUALITY", typeof(char));
            crustStressTable.Columns.Add("REGIME", typeof(string));
            crustStressTable.Columns.Add("LOCALITY", typeof(string));
            crustStressTable.Columns.Add("COUNTRY", typeof(string));
            crustStressTable.Columns.Add("DATE", typeof(DateOnly));
            crustStressTable.Columns.Add("TIME", typeof(TimeOnly));
            crustStressTable.Columns.Add("NUMBER", typeof(int));
            crustStressTable.Columns.Add("SD", typeof(decimal));
            crustStressTable.Columns.Add("TOT_LEN", typeof(decimal));
            crustStressTable.Columns.Add("VENT", typeof(string));
            crustStressTable.Columns.Add("TOP", typeof(decimal));
            crustStressTable.Columns.Add("BOT", typeof(decimal));
            crustStressTable.Columns.Add("ANISOTROPY", typeof(string));
            crustStressTable.Columns.Add("METHOD", typeof(string));
            crustStressTable.Columns.Add("S1AZ", typeof(decimal));
            crustStressTable.Columns.Add("S1PL", typeof(decimal));
            crustStressTable.Columns.Add("S2AZ", typeof(decimal));
            crustStressTable.Columns.Add("S2PL", typeof(decimal));
            crustStressTable.Columns.Add("S3AZ", typeof(decimal));
            crustStressTable.Columns.Add("S3PL", typeof(decimal));
            crustStressTable.Columns.Add("MAG_TYPE", typeof(string));
            crustStressTable.Columns.Add("EQ_MAG", typeof(decimal));
            crustStressTable.Columns.Add("CRUST", typeof(string));
            crustStressTable.Columns.Add("REF1", typeof(string));
            crustStressTable.Columns.Add("REF2", typeof(string));
            crustStressTable.Columns.Add("REF3", typeof(string));
            crustStressTable.Columns.Add("REF4", typeof(string));
            crustStressTable.Columns.Add("REF5", typeof(string));
            crustStressTable.Columns.Add("REF6", typeof(string));
            crustStressTable.Columns.Add("COMMENT", typeof(string));
            crustStressTable.Columns.Add("PLATE", typeof(string));
            crustStressTable.Columns.Add("DIST", typeof(decimal));

            foreach(CrustStress crustStress in crustStressList)
            {
                crustStressTable.Rows.Add(
                    crustStress.Id,
                    crustStress.Isc_Id,
                    crustStress.Site,
                    crustStress.Latitude,
                    crustStress.Longitude,
                    crustStress.Azimuth,
                    crustStress.Type,
                    crustStress.Depth,
                    crustStress.Quality,
                    crustStress.Regime,
                    crustStress.Locality,
                    crustStress.Country,
                    crustStress.Date,
                    crustStress.Time,
                    crustStress.Number,
                    crustStress.SD,
                    crustStress.TotalLength,
                    crustStress.Vent,
                    crustStress.TopDepth,
                    crustStress.BottomDepth,
                    crustStress.Anisotropy,
                    crustStress.S1az,
                    crustStress.S1pl,
                    crustStress.S2az,
                    crustStress.S2pl,
                    crustStress.S3az,
                    crustStress.S3pl,
                    crustStress.MagnitudeType,
                    crustStress.EarthquakeMagnitude,
                    crustStress.Crust,
                    crustStress.Reference1,
                    crustStress.Reference2,
                    crustStress.Reference3,
                    crustStress.Reference4,
                    crustStress.Reference5,
                    crustStress.Reference6,
                    crustStress.Comment,
                    crustStress.Plate,
                    crustStress.Dist
                );
            }

            DynamicParameters? parameters = new DynamicParameters();
            parameters.Add(
                 "@CrustStress",
                 crustStressTable.AsTableValuedParameter("dbo.CrustStress")
            );

            await sql.ExecuteAsync(
                "dbo.InsertCrustStress",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

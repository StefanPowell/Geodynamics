using Dapper;
using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EarthQuake.Persistence.Repository;

public class EarthquakeRepository : IEarthquakeRepository
{
    private readonly string _connectionString;

    public EarthquakeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        SqlConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    public async Task SaveData(List<Feature> data)
    {
        try
        {
            using (var sql = CreateConnection())
            {
                DataTable Identifiers = new DataTable();
                Identifiers.Columns.Add("Id", typeof(string));
                Identifiers.Columns.Add("Type", typeof(string));

                DataTable Geo = new DataTable();
                Geo.Columns.Add("latitude", typeof(double));
                Geo.Columns.Add("longitude", typeof(double));
                Geo.Columns.Add("depth", typeof(double));
                Geo.Columns.Add("type", typeof(string));

                DataTable Properties = new DataTable();
                Properties.Columns.Add("mag", typeof(float));
                Properties.Columns.Add("place", typeof(string));
                Properties.Columns.Add("time", typeof(float));
                Properties.Columns.Add("updated", typeof(string));
                Properties.Columns.Add("tz", typeof(string));
                Properties.Columns.Add("url", typeof(string));
                Properties.Columns.Add("detail", typeof(string));
                Properties.Columns.Add("felt", typeof(string));
                Properties.Columns.Add("cdi", typeof(string));
                Properties.Columns.Add("mmi", typeof(float));
                Properties.Columns.Add("alert", typeof(string));
                Properties.Columns.Add("status", typeof(string));
                Properties.Columns.Add("tsunami", typeof(int));
                Properties.Columns.Add("sig", typeof(int));
                Properties.Columns.Add("net", typeof(string));
                Properties.Columns.Add("code", typeof(string));
                Properties.Columns.Add("ids", typeof(string));
                Properties.Columns.Add("sources", typeof(string));
                Properties.Columns.Add("types", typeof(string));
                Properties.Columns.Add("nst", typeof(int));
                Properties.Columns.Add("dmin", typeof(float));
                Properties.Columns.Add("rms", typeof(float));
                Properties.Columns.Add("gap", typeof(float));
                Properties.Columns.Add("magType", typeof(string));
                Properties.Columns.Add("type", typeof(string));
                Properties.Columns.Add("title", typeof(string));

                foreach (var feature in data)
                {
                    Identifiers.Rows.Add(
                            feature.id,
                            feature.type
                        );

                    Geo.Rows.Add(
                        feature.geometry.coordinates[1],
                        feature.geometry.coordinates[0],
                        feature.geometry.coordinates[2],
                        feature.geometry.type
                    );

                    Properties.Rows.Add(
                        feature.properties.mag,
                        feature.properties.place,
                        feature.properties.time,
                        feature.properties.updated,
                        feature.properties.tz,
                        feature.properties.url,
                        feature.properties.detail,
                        feature.properties.felt,
                        feature.properties.cdi,
                        feature.properties.mmi,
                        feature.properties.alert,
                        feature.properties.status,
                        feature.properties.tsunami,
                        feature.properties.sig,
                        feature.properties.net,
                        feature.properties.code,
                        feature.properties.ids,
                        feature.properties.sources,
                        feature.properties.types,
                        feature.properties.nst,
                        feature.properties.dmin,
                        feature.properties.rms,
                        feature.properties.gap,
                        feature.properties.magType,
                        feature.properties.type,
                        feature.properties.title
                    );
                }

                var parameters = new DynamicParameters();
                parameters.Add("@Identifiers", Identifiers.AsTableValuedParameter("dbo.Identifiers"));
                parameters.Add("@Geo", Geo.AsTableValuedParameter("dbo.Geo"));
                parameters.Add("@Properties", Properties.AsTableValuedParameter("dbo.Properties"));

                await sql.ExecuteAsync("dbo.InsertFeatures", parameters, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
    }

    public async Task<List<DBFeature>> GetLatestQuakes(int totalValues)
    {
        using (var sql = CreateConnection())
        {
            var parameters = new { TotalValues = totalValues };

            IEnumerable<DBFeature> result = await sql.QueryAsync<DBFeature>(
                "dbo.GetLatestFeatures",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.ToList();
        }
    }

    public async Task SaveWaveFormData(List<miniSEED> WaveFormData)
    {

    }

    public async Task UpdateFaultStress(FaultStress faultStress)
    {
        using var sql = CreateConnection();

        // Create a DataTable matching the table type
        var table = new DataTable();
        table.Columns.Add("faultName", typeof(string));
        table.Columns.Add("slipRate_m_per_yr", typeof(double));
        table.Columns.Add("lockedThickness_m", typeof(double));
        table.Columns.Add("ruptureLength_m", typeof(double));
        table.Columns.Add("shearStressRate_MPa_per_yr", typeof(double));
        table.Columns.Add("accumulatedStress_MPa", typeof(double));
        table.Columns.Add("expectedSlip_m", typeof(double));
        table.Columns.Add("momentMagnitude_Mw", typeof(double));

        // Add the faultStress row
        table.Rows.Add(
            faultStress.FaultName,
            faultStress.SlipRate_m_per_yr,
            faultStress.LockedThickness_m,
            faultStress.RuptureLength_m,
            faultStress.ShearStressRate_MPa_per_yr,
            faultStress.AccumulatedStress_MPa,
            faultStress.ExpectedSlip_m,
            faultStress.MomentMagnitude_Mw
        );

        // Create SqlParameter for TVP
        var parameter = new SqlParameter("@FaultStressList", SqlDbType.Structured)
        {
            TypeName = "dbo.FaultStressType",
            Value = table
        };

        await sql.ExecuteAsync(
            "dbo.UpdateFaultStress",
            new { FaultStressList = parameter.Value },
            commandType: CommandType.StoredProcedure
        );
    }
}

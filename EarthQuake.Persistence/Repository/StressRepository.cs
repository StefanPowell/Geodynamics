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
            crustStressTable.Columns.Add("ISC_ID", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("SITE", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("LAT", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("LON", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("AZI", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("TYPE", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("DEPTH", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("QUALITY", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REGIME", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("LOCALITY", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("COUNTRY", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("DATE", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("TIME", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("NUMBER", typeof(int)).AllowDBNull = true;
            crustStressTable.Columns.Add("SD", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("TOT_LEN", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("VENT", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("TOP", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("BOT", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("ANISOTROPY", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("METHOD", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("S1AZ", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("S1PL", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("S2AZ", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("S2PL", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("S3AZ", typeof(decimal)).AllowDBNull = true;
            crustStressTable.Columns.Add("S3PL", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("MAG_TYPE", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("EQ_MAG", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("CRUST", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF1", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF2", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF3", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF4", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF5", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("REF6", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("COMMENT", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("PLATE", typeof(string)).AllowDBNull = true;
            crustStressTable.Columns.Add("DIST", typeof(decimal)).AllowDBNull = true;


            foreach (CrustStress crustStress in crustStressList)
            {


                crustStressTable.Rows.Add(
                    ToDbValue(crustStress.ID),
                    ToDbValue(crustStress.ISC_ID),
                    ToDbValue(crustStress.SITE),
                    SqlDecimal(crustStress.LAT),
                    SqlDecimal(crustStress.LON),
                    ToDbValue(crustStress.AZI),
                    ToDbValue(crustStress.TYPE),
                    ToDbValue(crustStress.DEPTH),
                    ToDbValue(crustStress.QUALITY),
                    ToDbValue(crustStress.REGIME),
                    ToDbValue(crustStress.LOCALITY),
                    ToDbValue(crustStress.COUNTRY),
                    ToDbValue(crustStress.DATE),
                    ToDbValue(crustStress.TIME),
                    ToDbValue(crustStress.NUMBER),
                    SqlDecimal(crustStress.SD),
                    SqlDecimal(crustStress.TOT_LEN),
                    ToDbValue(crustStress.VENT),
                    SqlDecimal(crustStress.TOP),
                    SqlDecimal(crustStress.BOT),
                    ToDbValue(crustStress.ANISOTROPY),
                    SqlDecimal(crustStress.S1AZ),
                    SqlDecimal(crustStress.S1PL),
                    SqlDecimal(crustStress.S2AZ),
                    SqlDecimal(crustStress.S2PL),
                    SqlDecimal(crustStress.S3AZ),
                    ToDbValue(crustStress.S3PL),
                    ToDbValue(crustStress.MAG_TYPE),
                    ToDbValue(crustStress.EQ_MAG),
                    ToDbValue(crustStress.CRUST),
                    ToDbValue(crustStress.REF1),
                    ToDbValue(crustStress.REF2),
                    ToDbValue(crustStress.REF3),
                    ToDbValue(crustStress.REF4),
                    ToDbValue(crustStress.REF5),
                    ToDbValue(crustStress.REF6),
                    ToDbValue(crustStress.COMMENT),
                    ToDbValue(crustStress.PLATE),
                    SqlDecimal(crustStress.DIST)
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

    private object ToDbValue(object? value)
    {
        return value ?? DBNull.Value;
    }

    private object SqlDecimal(object? value)
    {
        // 1. If it's already a null or DBNull, return DBNull
        if (value == null || value == DBNull.Value) return DBNull.Value;

        // 2. If it's a string, try to parse it. If empty or invalid, return DBNull
        if (value is string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return DBNull.Value;
            return decimal.TryParse(str, out decimal result) ? result : DBNull.Value;
        }

        // 3. If it's already a numeric type, return it
        if (value is decimal || value is double || value is float || value is int)
            return value;

        return DBNull.Value;
    }
}

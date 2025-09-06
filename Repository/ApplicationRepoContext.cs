using EarthQuake.Models;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EarthQuake.Computations;
using EarthQuake.Interface;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace EarthQuake.Repository
{
    public class ApplicationRepoContext : IApplicationRepoContext
    {
        private List<Feature> _repodatabase;
        private readonly string _connectionString;

        public ApplicationRepoContext(IConfiguration config)
        {
            _repodatabase = new List<Feature>();
            _connectionString = config.GetConnectionString("DefaultConnection")
                                ?? throw new ArgumentNullException(nameof(config), "Connection string not found.");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void SaveData(List<Feature> data)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    using (var cmd = new SqlCommand("quake.InsertFeatures", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var table = new DataTable();
                        table.Columns.Add("Type", typeof(string));
                        table.Columns.Add("PropertiesId", typeof(int));
                        table.Columns.Add("GeoId", typeof(int));
                        table.Columns.Add("Id", typeof(string));

                        var tableParam = new SqlParameter("@Features", SqlDbType.Structured)
                        {
                            TypeName = "FeatureType", // matches your SQL table type
                            Value = table      // your DataTable matching FeatureType
                        };

                        cmd.Parameters.Add(tableParam);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public void DeleteData(Feature datapoint) {
            _repodatabase.Remove(datapoint);
        }

        public List<Feature> GetBetweenDates(DateOnly startdate, DateOnly enddate)
        {

            long startdateEpoch = convertToEpoch(startdate);
            long enddateEpoch = convertToEpoch(enddate);
            List<Feature> x = _repodatabase.Where(x => (long)x.properties.time > startdateEpoch && (long)x.properties.time < enddateEpoch).ToList();
            return x;
        }

        public List<Feature> GetRepodatabase() {
            List<Feature> xx = _repodatabase;
            return _repodatabase; 
        }

        public long convertToEpoch(DateOnly date)
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            return (long)(date.ToDateTime(TimeOnly.MinValue) - epoch).TotalSeconds;
        }

        //save data to databaase
        public async void SaveEarthQuakeFeatureListAsync(List<Feature> quakes)
        {
            const string sql = @"
                INSERT INTO faults (name, type, dip, last_movement)
                VALUES (@Name, @Type, @Dip, @LastMovement)";

            using var connection = CreateConnection();
            await connection.ExecuteAsync(sql, quakes);
        }

        //delete data from database

        //get between dates data from database
    }
}

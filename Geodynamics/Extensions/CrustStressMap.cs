using CsvHelper.Configuration;
using EarthQuake.Persistence.Models;
using System.Globalization;

namespace EarthQuake.Extensions
{
    public class CrustStressMap : ClassMap<CrustStress>
    {
        public CrustStressMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            // Tell CsvHelper that an empty cell means NULL for these properties
            Map(m => m.S1AZ).TypeConverterOption.NullValues("");
            Map(m => m.S1PL).TypeConverterOption.NullValues("");
            Map(m => m.S2AZ).TypeConverterOption.NullValues("");
            Map(m => m.S2PL).TypeConverterOption.NullValues("");
            Map(m => m.S3AZ).TypeConverterOption.NullValues("");
            Map(m => m.S3PL).TypeConverterOption.NullValues("");

            // Add for other numeric columns as needed
            Map(m => m.LAT).TypeConverterOption.NullValues("");
            Map(m => m.LON).TypeConverterOption.NullValues("");

            Map(m => m.DATE).Optional();
            Map(m => m.TIME).Optional();
        }
    }


}

using EarthQuake.Persistence.Enum;
using EarthQuake.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Repository.Abstractions
{
    public interface IDataQueryRepository
    {
        Task<IEnumerable<DBFeature>> GetNumberofEarthquakeEventsInGridArea(double latitudeStart, double latitudeEnd, double longitudeStart, double longitudeEnd, DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<Feature>> GetNumberofEarthquakeEventsInCircularArea(double latitude, double longitude, double radius, UnitOfMeasure unit, DateOnly startdate, DateOnly enddate);
        Task<List<int>> GetQuakesNeedingWaveformCheck();
        Task RemoveQuakeFromWaveFormCheckList(int featureId);
        Task InsertStationWithFeatureWaveformData(int featureId, StationData stationData);
    }
}

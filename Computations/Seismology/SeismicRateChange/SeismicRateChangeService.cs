using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Enum;
using EarthQuake.Interface;
using EarthQuake.Models;
using EarthQuake.Models.GeographicalBoundaryTypes;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;
using System.Web.WebPages;

namespace EarthQuake.Computations.Seismology.SeismicRateChange;

public class SeismicRateChangeService : ISeismicRateChangeService
{
    private readonly IDataQueryRepository _dataQueryRepository;

    public SeismicRateChangeService(IDataQueryRepository dataQueryRepository)
    {
        _dataQueryRepository = dataQueryRepository;
    }

    public async Task<double> GetSeismicRateChange(GeographicalBoundary geographicalBoundary, DateOnly startingSeismicRateStartDate, DateOnly endingSeismicRateStartDate, TimePeriod timePeriod, int timePeriodCount)
    {
        SeismicRate startingSeismicRate = new SeismicRate
        {
            geographicalBoundary = geographicalBoundary,
            startdate = startingSeismicRateStartDate,
            timePeriod = timePeriod,
            timePeriodCount = timePeriodCount
        };
        SeismicRate endingSeismicRate = new SeismicRate
        {
            geographicalBoundary = geographicalBoundary,
            startdate = endingSeismicRateStartDate,
            timePeriod = timePeriod,
            timePeriodCount = timePeriodCount
        };

        double startingSeismicRateChange = await GetSeismicRateAsync(startingSeismicRate);
        double endingSeismicRateChange = await GetSeismicRateAsync(endingSeismicRate);

        return GetRateChange(startingSeismicRateChange, endingSeismicRateChange);
    }


    public double GetRateChange(double r1, double r2)
    {
        return (((r2 - r1) / r1) * 100);
    }

    public async Task<int> GetSeismicRateAsync(SeismicRate seismicRate)
    {
        return (await GetNumberofEventsAsync(seismicRate.startdate, seismicRate.timePeriod, seismicRate.timePeriodCount, seismicRate.geographicalBoundary));
    }

    public async Task<int> GetNumberofEventsAsync(DateOnly startdate, TimePeriod timePeriod, int timePeriodCount, GeographicalBoundary geographicalBoundary)
    {
        DateOnly enddate = GetEndDate(startdate, timePeriod, timePeriodCount);

        if (geographicalBoundary.boundaryType == GeographicalBoundaryType.FaultZone)
        {
            throw new NotImplementedException();
        }
        else if (geographicalBoundary.boundaryType == GeographicalBoundaryType.GridCell)
        {
            GridCell gridCell = (GridCell)geographicalBoundary.boundaryObject;
            int totalevents = await GetNumberofEarthquakeEventsInGridArea(gridCell, startdate, enddate);
            return totalevents;
        }
        else if (geographicalBoundary.boundaryType == GeographicalBoundaryType.CircularArea)
        {
            CircularArea circularArea = (CircularArea)geographicalBoundary.boundaryObject;
            int totalevents = await GetNumberofEarthquakeEventsInCircularArea(circularArea, startdate, enddate);
            return totalevents;
        }
        else
        {
            throw new ArgumentException("Invalid Geographical Boundary Type specified.", nameof(GeographicalBoundaryType));
        }
    }

    public DateOnly GetEndDate(DateOnly startDate, TimePeriod timePeriod, int timePeriodCount)
    {
        if (timePeriodCount <= 0)
        {
            return startDate;
        }

        DateOnly endOfPeriodStart;

        if (timePeriod == TimePeriod.Daily)
        {
            endOfPeriodStart = startDate.AddDays(timePeriodCount);
        }
        else if (timePeriod == TimePeriod.Weekly)
        {
            endOfPeriodStart = startDate.AddDays(7 * timePeriodCount);
        }
        else if (timePeriod == TimePeriod.Monthly)
        {
            endOfPeriodStart = startDate.AddMonths(timePeriodCount);
        }
        else if (timePeriod == TimePeriod.Yearly)
        {
            endOfPeriodStart = startDate.AddYears(timePeriodCount);
        }
        else
        {
            throw new ArgumentException("Invalid parameters specified.", nameof(timePeriod));
        }
        return endOfPeriodStart.AddDays(-1);
    }


    public async Task<int> GetNumberofEarthquakeEventsInGridArea(GridCell gridCell, DateOnly startdate, DateOnly enddate)
    {
        IEnumerable<Feature> allQuakeEvents =  await _dataQueryRepository.GetNumberofEarthquakeEventsInGridArea(gridCell.startLatitude, gridCell.endLatitude, gridCell.startLongitude, gridCell.endLongitude, startdate, enddate);
        return allQuakeEvents.Count();
    }

    public async Task<int> GetNumberofEarthquakeEventsInCircularArea(CircularArea circularArea, DateOnly startdate, DateOnly enddate)
    {
        IEnumerable<Feature> allQuakeEvents = await _dataQueryRepository.GetNumberofEarthquakeEventsInCircularArea(circularArea.Coordinate.latitude, circularArea.Coordinate.longitude, circularArea.radius, circularArea.unit, startdate, enddate);
        return allQuakeEvents.Count();
    }
}

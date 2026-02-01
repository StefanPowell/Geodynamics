using EarthQuake.Computations.Seismology.Models;
using EarthQuake.Enum;
using EarthQuake.Interface;
using EarthQuake.Models;
using EarthQuake.Models.GeographicalBoundaryTypes;
using EarthQuake.Persistence.Models;
using EarthQuake.Persistence.Repository.Abstractions;

namespace EarthQuake.Computations.Seismology.GutenRitcher;

public class GutenRitcher : IGutenRitcher
{
    private readonly IDataQueryRepository _dataQueryRepository;

    public GutenRitcher(IDataQueryRepository dataQueryRepository)
    {
        _dataQueryRepository = dataQueryRepository;
    }

    public async Task<GutenRitcherInfo> GetGutenRitcherData(GeographicalBoundary geographicalBoundary, DateOnly startDate, DateOnly endDate, int magnitudeCompletness)
    {
        int allObserverdEarthQuakes = await AllObserverdEarthQuakes(geographicalBoundary, startDate, endDate, magnitudeCompletness);
        double probabilityOfEarthquakeOfSpecifiedMagnitudeOrHigher = await ProbabilityOfEarthquakeOfSpecifiedMagnitudeOrHigher(geographicalBoundary, startDate, endDate, magnitudeCompletness);
        double totalNumberOfEarthQuakesInTimePeriod = allObserverdEarthQuakes / probabilityOfEarthquakeOfSpecifiedMagnitudeOrHigher;
        GutenRitcherInfo gutenRitcherInfo = new GutenRitcherInfo { 
            MinimumMagnitude = magnitudeCompletness,
            NumberOfEvents = allObserverdEarthQuakes,
            ProbabiltyOfEarthquake = probabilityOfEarthquakeOfSpecifiedMagnitudeOrHigher,
            TotalNumberThatOccured = totalNumberOfEarthQuakesInTimePeriod
        };

        return gutenRitcherInfo;
    }

    public async Task<double> ProbabilityOfEarthquakeOfSpecifiedMagnitudeOrHigher(GeographicalBoundary geographicalBoundary, DateOnly startDate, DateOnly endDate, int magnitudeCompletness)
    {
        double averageMagnitudeOfQuakes = await GetAverageMagnitudeOfQuakes(geographicalBoundary, startDate, endDate, magnitudeCompletness);

        double bValue = (0.434)/(averageMagnitudeOfQuakes - (magnitudeCompletness - (.1 / 2))); //Maximum Likelihood formula
        //  b ≈ 1 → normal
        //  b < 1 → more big earthquakes than usual → stress may be building
        //  b > 1 → mostly small earthquakes → system is releasing energy in small amounts

        return Math.Pow(10, -(bValue * magnitudeCompletness));
    }

    public async Task<int> AllObserverdEarthQuakes(GeographicalBoundary geographicalBoundary, DateOnly startDate, DateOnly endDate, int magnitudeCompletness)
    {
        //you are using two things, fix it in the repo
        IEnumerable<DBFeature> allQuakes;
        IEnumerable<Feature> allQuakesX;

        if (geographicalBoundary.boundaryType == GeographicalBoundaryType.FaultZone)
        {
            throw new NotImplementedException();
        }
        else if (geographicalBoundary.boundaryType == GeographicalBoundaryType.GridCell)
        {
            GridCell gridCell = (GridCell)geographicalBoundary.boundaryObject;
            allQuakes = await _dataQueryRepository.GetNumberofEarthquakeEventsInGridArea(gridCell.startLatitude, gridCell.endLatitude, gridCell.startLongitude, gridCell.endLongitude, startDate, endDate);
            return allQuakes.Where(earthquake => earthquake.Magnitude >= magnitudeCompletness).Count();
        }
        else if (geographicalBoundary.boundaryType == GeographicalBoundaryType.CircularArea)
        {
            CircularArea circularArea = (CircularArea)geographicalBoundary.boundaryObject;
            allQuakesX = await _dataQueryRepository.GetNumberofEarthquakeEventsInCircularArea(circularArea.Coordinate.latitude, circularArea.Coordinate.longitude, circularArea.radius, circularArea.unit, startDate, endDate);
            return allQuakesX.Where(earthquake => earthquake.properties.mag >= magnitudeCompletness).Count();
        }
        else
        {
            throw new ArgumentException("Invalid Geographical Boundary Type specified.", nameof(GeographicalBoundaryType));
        }
    }

    public async Task<double> GetAverageMagnitudeOfQuakes(GeographicalBoundary geographicalBoundary, DateOnly startDate, DateOnly endDate, double magnitudeCompletness)
    {
        //you are using two things, fix it in the repo
        IEnumerable<DBFeature> allQuakes;
        IEnumerable<Feature> allQuakesX;

        if (geographicalBoundary.boundaryType == GeographicalBoundaryType.FaultZone)
        {
            throw new NotImplementedException();
        }
        else if (geographicalBoundary.boundaryType == GeographicalBoundaryType.GridCell)
        {
            GridCell gridCell = (GridCell)geographicalBoundary.boundaryObject;
            allQuakes = await _dataQueryRepository.GetNumberofEarthquakeEventsInGridArea(gridCell.startLatitude, gridCell.endLatitude, gridCell.startLongitude, gridCell.endLongitude, startDate, endDate);
            return allQuakes.Where(earthquake => earthquake.Magnitude >= magnitudeCompletness).Average(quake => quake.Magnitude);
        }
        else if(geographicalBoundary.boundaryType == GeographicalBoundaryType.CircularArea)
        {
            CircularArea circularArea = (CircularArea)geographicalBoundary.boundaryObject;
            allQuakesX = await _dataQueryRepository.GetNumberofEarthquakeEventsInCircularArea(circularArea.Coordinate.latitude, circularArea.Coordinate.longitude, circularArea.radius, circularArea.unit, startDate, endDate);
            return allQuakesX.Where(earthquake => earthquake.properties.mag >= magnitudeCompletness).Average(quake => quake.properties.mag);
        }
        else
        {
            throw new ArgumentException("Invalid Geographical Boundary Type specified.", nameof(GeographicalBoundaryType));
        }
    }

}

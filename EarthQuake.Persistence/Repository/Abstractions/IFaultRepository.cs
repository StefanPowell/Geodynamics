using EarthQuake.Models;

namespace EarthQuake.Persistence.Repository.Abstractions;

public interface IFaultRepository
{
    Task<Fault> GetNearestFaultToCoordinates(double latitude, double longitude);
    Task SaveFaultData(List<Fault> faults);
}

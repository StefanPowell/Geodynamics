using EarthQuake.Models;
using Microsoft.Data.SqlClient;

namespace EarthQuake.Interface
{
    public interface IFaultRepository
    {
        SqlConnection CreateConnection();

        Task<IEnumerable<Fault>> GetAllFaultsAsync();

        Task<int> AddFaultAsync(Fault fault);
    }
}

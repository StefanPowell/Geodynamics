using EarthQuake.Models;
using EarthQuake.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Repository.Abstractions
{
    public interface IEarthquakeRepository
    {
        Task SaveData(List<Feature> data);
        Task<List<DBFeature>> GetLatestQuakes(int totalValues);
        Task SaveWaveFormData(List<miniSEED> WaveFormData);
        Task UpdateFaultStress(FaultStress faultStress);
    }
}

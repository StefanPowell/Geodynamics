using EarthQuake.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarthQuake.Persistence.Repository.Abstractions;

public interface IStressRepository
{
    Task SaveCrustStressField(List<CrustStress> crustStressList);
}

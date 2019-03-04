using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Interfaces
{
    public interface IDistancesLogic
    {
        Task<IEnumerable<Distances>> GetAllDistancesAsync();

        Task<Distances> GetByDistanceIdAsync(string id);

        Task CreateDistanceAsync(DistancesModel distances);

        Task<bool> DeleteDistanceAsync(string id);

        Task<bool> UpdateDistanceAsync(DistancesModel distances, string id);
    }
}

/**************************************************************************
 *                                                                        *
 *  File:        IDistancesLogic.cs                                       *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Interfaces
{

    /// <summary>
    /// IDistancesLogic interface.
    /// </summary>
    /// <remarks>
    /// Get necesarry data from Data Access Layer (DAL) and expose to web API.
    /// </remarks>
    public interface IDistancesLogic
    {
        Task<IEnumerable<Distances>> GetAllDistancesAsync();

        Task<Distances> GetByDistanceIdAsync(string id);

        Task CreateDistanceAsync(DistancesModel distances);

        Task<bool> DeleteDistanceAsync(string id);

        Task<DistancesModel> GetRoadDistanceAsync(string startCity, string destinationCity);

        Task<bool> UpdateDistanceAsync(DistancesModel distances, string id);
    }
}

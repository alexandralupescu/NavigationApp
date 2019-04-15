/**************************************************************************
 *                                                                        *
 *  File:        IDistancesLogic.cs                                       *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 *                                                                        *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Interfaces
{

    /// <summary>
    /// IDistancesLogic interface
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

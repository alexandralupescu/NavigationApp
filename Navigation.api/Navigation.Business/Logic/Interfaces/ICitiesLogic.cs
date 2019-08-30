/**************************************************************************
 *                                                                        *
 *  File:        ICitiesLogic.cs                                          *
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
    /// ICitiesLogic interface.
    /// </summary>
    /// <remarks>
    /// Get necesarry data from Data Access Layer (DAL) and expose to web API.
    /// </remarks>
    public interface ICitiesLogic
    {
        Task<IEnumerable<Cities>> GetAllCitiesAsync();

        Task<Cities> GetByCityIdAsync(string id);

        Task CreateCityAsync(CitiesModel cities);

        Task<bool> DeleteCityAsync(string id);

        Task<CitiesModel> GetCityByNameAsync(string cityName);

        Task<bool> UpdateCityAsync(CitiesModel cities,string id);

        Task<bool> FindCityNameAsync(string cityName);
    }
}

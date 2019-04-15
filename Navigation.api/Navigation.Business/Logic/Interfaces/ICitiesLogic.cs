/**************************************************************************
 *                                                                        *
 *  File:        ICitiesLogic.cs                                          *
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
using MongoDB.Bson;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Text;
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

        Task<CitiesModel> GetCityByName(string startCity);

        Task<bool> UpdateCityAsync(CitiesModel cities,string id);


    }
}

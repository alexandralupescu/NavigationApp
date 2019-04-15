/**************************************************************************
 *                                                                        *
 *  File:        CitiesLogic.cs                                           *
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
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Implementations
{
 
    /// <summary>
    /// CitiesLogic implements the ICitiesLogic interface and calls the methods from DBService class.
    /// </summary>
    /// <remarks>
    /// BLL (Business Logic Layer) serves as an intermediary layer for data exchange between the presentation layer and DAL (Data Access Layer).
    /// </remarks>
    public class CitiesLogic : ICitiesLogic
    {

        #region Private Members
        /// <summary>
        /// Used to access methods from DbService class.
        /// </summary>
        private readonly IDbService<Cities> _cityService;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor that contains the instance of an IDBService object.
        /// </summary>
        /// <param name="cityService">Instance of Cities object</param>
        public CitiesLogic(IDbService<Cities> cityService)
        {
            _cityService = cityService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inserts a new city in Cities collection.
        /// </summary>
        /// <param name="cities">Document that will be submited in Cities collection.</param>
        public async Task CreateCityAsync(CitiesModel cities)
        {
            try
            {
                var @city = new Cities
                {
                    CityName = cities.CityName,
                    Latitude = cities.Latitude,
                    Longitude = cities.Longitude,
                    IsResidence = cities.IsResidence,
                    County = cities.County
                };


                await _cityService.CreateAsync(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in CitiesLogic - CreateCityAsync(cities) method!"), exception);
            }
        }

        /// <summary>
        /// Deletes a single city document mathcing the provided search criteria.
        /// </summary>
        /// <param name="id">The matching criteria for delete operation.</param>
        public async Task<bool> DeleteCityAsync(string id)
        {
            try
            {
                return await _cityService.DeleteAsync(id);
            }

            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in CitiesLogic - DeleteCityAsync(id) method!"), exception);
            }
        }

        /// <summary>
        /// Returns all documents from Cities collection.
        /// </summary>
        public async Task<IEnumerable<Cities>> GetAllCitiesAsync()
        {
            try
            {             
                return await _cityService.GetAllAsync();
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in CitiesLogic - GetAllCitiesAsync() method!"), exception);
            }
        }

        /// <summary>
        /// Returns the required city matching the provided search criteria, in this case city <b>id</b>
        /// </summary>
        /// <param name="id">The provided search criteria.</param>
        public Task<Cities> GetByCityIdAsync(string id)
        {
            try
            {
                return _cityService.GetByIdAsync(id);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesLogic - GetByCityIdAsync(id) method!"), exception);
            }
        }

        /// <summary>
        /// Returns the required informations mathcing the provided search criteria, in our case, the start city.
        /// </summary>
        /// <param name="startCity">The city from which to start.</param>
        /// <returns></returns>
        public async Task<CitiesModel> GetCityByName(string startCity)
        {
            try
            {
                var saveCity = await _cityService.GetAllAsync();
                foreach (Cities city in saveCity)
                {
                    if (city.CityName.Contains(startCity))
                    {
                        var citySave = new CitiesModel
                        {
                            CityName = city.CityName,
                            Latitude = city.Latitude,
                            Longitude = city.Longitude,
                            IsResidence = city.IsResidence,
                            County = city.County
                        };

                        return citySave;
                    }
                }

                return null;


            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - GetCityByName(startCity) method!"), exception);
            }
        }

        /// <summary>
        /// Replaces the city document matching the provided search criteria with the provided object.
        /// </summary>
        /// <param name="cities">Document that will be updated in Cities collection.</param>
        /// <param name="id">The provided search criteria.</param>
        public async Task<bool> UpdateCityAsync(CitiesModel cities,string id)
        {
            try
            {
                var cityFromDb = await _cityService.GetByIdAsync(id);

                var @city = new Cities
                {

                    CityName = cities.CityName,
                    Latitude = cities.Latitude,
                    Longitude = cities.Longitude,
                    IsResidence = cities.IsResidence,
                    County = cities.County,
                    Id = cityFromDb.Id
                };

               
                return await _cityService.UpdateAsync(city);
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesLogic - UpdateCityAsync(cities,id) method!"), exception);
            }
        }
        #endregion

    }
}

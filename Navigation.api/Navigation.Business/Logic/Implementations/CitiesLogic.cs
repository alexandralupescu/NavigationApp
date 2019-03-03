using MongoDB.Bson;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Implementations
{
   
    public class CitiesLogic : ICitiesLogic
    {
        #region Members
        /* used to access methods from DbService class */
        private readonly IDbService<Cities> _cityService;
        #endregion

        #region Public Methods
        public CitiesLogic(IDbService<Cities> cityService)
        {
            _cityService = cityService;
        }

        /* inserts a new city in Cities collection */
        public async Task CreateCityAsync(CitiesModel cities)
        {
            try
            {             
                var @city = new Cities
                {
                    CityName = cities.CityName,
                    Latitude = cities.Latitude,
                    Longitude = cities.Longitude,
                    IsResidence = cities.IsResidence
                };


                await _cityService.CreateAsync(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in CitiesLogic - CreateCityAsync(cities) method!"), exception);
            }
        }

        /* deletes a single city document mathcing the provided search criteria */
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


        /* returns all cities from Cities collection */
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

        /* returns the required city matching the provided search criteria, in this case city id */
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

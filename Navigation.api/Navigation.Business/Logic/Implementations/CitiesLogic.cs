﻿/**************************************************************************
 *                                                                        *
 *  File:        CitiesLogic.cs                                           *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Business.Logic.Implementations
{
 
    /// <summary>
    /// CitiesLogic implements the ICitiesLogic interface and calls the methods from DBService class.
    /// </summary>
    /// <remarks>
    /// BLL (Business Logic Layer) serves as an intermediary layer for data exchange between the 
    /// presentation layer and DAL (Data Access Layer).
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
                  string.Format("Error in CitiesLogic class - CreateCityAsync method!"), exception);
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
                  string.Format("Error in CitiesLogic class - DeleteCityAsync method!"), exception);
            }
        }

        /// <summary>
        /// Method that will retrieve a response (true/false) if a city exists in database.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>True, if the city is found in the database, false if it's not found.</returns>
        public async Task<bool> FindCityNameAsync(string cityName)
        {
            try
            {
                var saveCity = await _cityService.GetAllAsync();
                foreach (Cities city in saveCity)
                {
                    if (RemoveDiacritics(RemoveDelimiter(city.CityName)).Contains(cityName))
                    {
                        
                        return true;
                    }
                }

                return false;
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in CitiesLogic class - FindCityNameAsync method!"), exception);
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
                    string.Format("Error in CitiesLogic class - GetAllCitiesAsync method!"), exception);
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
                   string.Format("Error in CitiesLogic class - GetByCityIdAsync method!"), exception);
            }
        }

        /// <summary>
        /// Returns the required informations mathcing the provided search criteria, in our case, the start city.
        /// </summary>
        /// <param name="cityName">Name of the searched city</param>
        /// <returns></returns>
        public async Task<CitiesModel> GetCityByNameAsync(string cityName)
        {
            try
            {
                var saveCity = await _cityService.GetAllAsync();
                foreach (Cities city in saveCity)
                {
                    if (RemoveDiacritics(RemoveDelimiter(city.CityName)).Contains(cityName))
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
                   string.Format("Error in CitiesLogic class - GetCityByNameAsync method!"), exception);
            }
        }

        /// <summary>
        /// Replaces the city document matching the provided search criteria with the provided object.
        /// </summary>
        /// <param name="cities">Document that will be updated in Cities collection.</param>
        /// <param name="id">The provided search criteria.</param>
        public async Task<bool> UpdateCityAsync(CitiesModel cities, string id)
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
                   string.Format("Error in CitiesLogic class - UpdateCityAsync method!"), exception);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Remove delimiters method by 2 criterias (- or space) of the searched city.
        /// </summary>
        /// <param name="cityName">Name of the searched city.</param>
        /// <returns></returns>
        private string RemoveDelimiter(string cityName)
        {
            string[] finalText;
            string finalResult = "";

            /* First letter of a string will be uppercase, but no change for the other letters. */
            cityName = char.ToUpper(cityName[0]) + cityName.Substring(1).ToLower();
            
            /* Split operation. */
            finalText = cityName.Split(new Char[] { '-', ' ' });
            foreach (string word in finalText)
            {
                finalResult += word;
            }

            return finalResult;

        }

        /// <summary>
        /// Convert <b>cityName</b> string that are in Romanian and the method is able to take out the 
        /// Romanian accent marks in the letters while keeping the letter.
        /// </summary>
        /// <param name="cityName">Name of the searched city.</param>
        /// <returns></returns>
        private static string RemoveDiacritics(string cityName)
        {
            /* Indicates that a Unicode string is normalized using full canonical decomposition. */
            var normalizedString = cityName.Normalize(NormalizationForm.FormD);
            /* Represents a mutable string of characters. */
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            /* Indicates that a Unicode string is normalized using full canonical decomposition,
             * followed by the replacement of sequences with their primary composites, if possible. */
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        #endregion
    }
}

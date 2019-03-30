using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Navigation.AStar.Implementations;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using Navigation.DataAccess.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.api.Controllers
{

    /// <summary>
    /// CitiesController is responsible for controlling the way that a user interacts with the application.
    /// </summary>
    /// <remarks>
    /// A controller contains the flow control logic.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {

        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        #endregion

        #region Constructors
        /// <summary>
        /// CitiesController constructor.
        /// </summary>
        /// <remarks>
        /// CitiesController has a dependency on ICitiesLogic because it delegates some 
        /// responsabilities to CitiesLogic class.
        /// </remarks>
        /// <param name="citiesLogic">Used to access the data from the Business layer</param>
        public CitiesController(ICitiesLogic citiesLogic,IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;

        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// The GET method requests a representation of the specified resource.
        /// </summary>
        /// <remarks>
        /// GET method will return to user the list of the cities.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new ObjectResult(await _citiesLogic.GetAllCitiesAsync());
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in CitiesController - Get() method!"), exception);
            }
        }

        /// <summary>
        /// The GET method requests a representation of the specified resource matching the provided search criteria.
        /// </summary>
        /// <remarks>
        /// GET method will return to user a specific city that will match the criteria <b>id</b>.
        /// </remarks>
        /// <param name="id">The matching criteria for request.</param>
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var city = await _citiesLogic.GetByCityIdAsync(id);
                return new ObjectResult(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Get(id) method!"), exception);
            }
        }

        /// <summary>
        /// The POST method is used to submit a new document to the specified resource.
        /// </summary>
        /// <remarks>
        /// POST method will submit a new city document in Cities collection.
        /// </remarks>
        /// <param name="city">Entity that will be submited in Cities collection.</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CitiesModel city)
        {
            try
            {
                await _citiesLogic.CreateCityAsync(city);
                return new ObjectResult(city);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Post(city) method!"), exception);
            }
        }

        /// <summary>
        /// The DELETE method deletes the specified resource that matches the provided search criteria.
        /// </summary>
        /// <remarks>
        /// DELETE method deletes a document from Cities collection that matches the provided search criteria.
        /// </remarks>
        /// <param name="id">The matching criteria for delete.</param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var cityFromDb = await _citiesLogic.GetByCityIdAsync(id);
                await _citiesLogic.DeleteCityAsync(id);
                return new OkResult();
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Delete(id) method!"), exception);
            }
        }

        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        /// <remarks>
        /// PUT method updates a document from Cities collection.
        /// </remarks>
        /// <param name="city">Entity that will be updated in Cities collection</param>
        /// <param name="id">The matching criteria for update</param>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put([FromBody]CitiesModel city,string id)
        {
            try
            {          
                await _citiesLogic.UpdateCityAsync(city,id);
                return new OkObjectResult(city);

            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - Put(city,id) method!"), exception);
            }
        }

        /// <summary>
        /// The GET method requests a representation of the specified resource matching the provided search criteria.
        /// </summary>
        /// <remarks>
        /// GET method will return to user a specific city that will match the criteria (the name of the city from which to start).
        /// </remarks>
        /// <param name="startCity">The matching criteria for request.</param>
        [HttpGet,Route("startCity={startCity}")]
        public async Task<IActionResult> GetStartCityInfo([FromRoute]string startCity)
        {
            try
            {
                var getCityInfo = await _citiesLogic.GetCityByName(startCity);
                return new OkObjectResult(getCityInfo);
                
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - GetStartCityInfo(startCity) method!"), exception);
            }
        }

        /// <summary>
        /// The GET method requests a representation of the specified resource matching the provided search criteria.
        /// </summary>
        /// <remarks>
        /// GET method will return to user a specific city that will match the criteria (the name of the city where it will arrive).
        /// </remarks>
        /// <param name="destinationCity">The matching criteria for request.</param>
        [HttpGet,Route("destinationCity={destinationCity}")]
        public async Task<IActionResult> GetDestinationCityInfo([FromRoute] string destinationCity)
        {
            try
            {
                var getCityInfo = await _citiesLogic.GetCityByName(destinationCity);
                return new OkObjectResult(getCityInfo);

            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in CitiesController - GetStartCityInfo(startCity) method!"), exception);
            }
        }
        #endregion

    }
}
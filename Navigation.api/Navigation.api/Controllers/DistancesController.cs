/**************************************************************************
 *                                                                        *
 *  File:        DistancesController.cs                                   *
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
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;

namespace Navigation.api.Controllers
{

    /// <summary>
    /// A controller is responsible for controlling the way that a user interacts with the application.
    /// </summary>
    /// <remarks>
    /// The Controller is responsible for controlling the application logic and acts as the 
    /// coordinator between the View and the Model. The Controller receives an input from 
    /// the users via the View, then processes the user's data with the help of Model and 
    /// passes the results back to the View.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class DistancesController : ControllerBase
    {

        #region Private Members
        /// <summary>
        ///  Used to access the data from the Business layer.
        /// </summary>
        private readonly IDistancesLogic _distancesLogic;
        #endregion

        #region Constructors
        /// <summary>
        /// DistancesController constructor.
        /// </summary>
        /// <remarks>
        /// DistancesController has a dependency on IDistancesLogic because it delegates some responsabilities to DistancesLogic class.
        /// </remarks>
        /// <param name="distancesLogic">Used to access the data from the Business layer.</param>
        public DistancesController(IDistancesLogic distancesLogic)
        {
            _distancesLogic = distancesLogic;
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// The GET method requests a representation of the specified resource.
        /// </summary>
        /// <remarks>
        /// GET method will return to user all the distances from the database.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return new ObjectResult(await _distancesLogic.GetAllDistancesAsync());
            }
            catch (Exception exception)
            {
                throw new Exception(
                    string.Format("Error in DistancesController - Get() method!"), exception);
            }
        }

        /// <summary>
        /// The GET method requests a representation of the specified resource matching the provided search criteria.
        /// </summary>
        /// <remarks>
        /// GET method will return to user a specific distance that will match the criteria <b>Id</b>.
        /// </remarks>
        /// <param name="id">The matching criteria for request.</param>
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var distance = await _distancesLogic.GetByDistanceIdAsync(id);
                return new ObjectResult(distance);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Get(id) method!"), exception);
            }
        }

        /// <summary>
        /// The POST method is used to submit a new document to the specified resource.
        /// </summary>
        /// <remarks>
        /// POST method will submit a new distance document in Distances collection.
        /// </remarks>
        /// <param name="distance">Entity that will be submited in Distances collection.</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DistancesModel distance)
        {
            try
            {
                await _distancesLogic.CreateDistanceAsync(distance);
                return new ObjectResult(distance);
            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Post(distance) method!"), exception);
            }
        }

        /// <summary>
        /// The DELETE method deletes the specified resource that matches the provided search criteria.
        /// </summary>
        /// <remarks>
        /// DELETE method deletes a document from Distances collection that matches the provided search criteria.
        /// </remarks>
        /// <param name="id">The matching criteria for delete.</param>
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            try
            {
                var distanceFromDb = await _distancesLogic.GetByDistanceIdAsync(id);
                await _distancesLogic.DeleteDistanceAsync(id);
                return new OkResult();
            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Delete(id) method!"), exception);
            }
        }

        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        /// <remarks>
        /// PUT method updates a document from Distances collection.
        /// </remarks>
        /// <param name="city">Entity that will be updated in Distances collection.</param>
        /// <param name="id">The matching criteria for update.</param>
        /// <returns></returns>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put([FromBody]DistancesModel distance, string id)
        {
            try
            {
                await _distancesLogic.UpdateDistanceAsync(distance, id);
                return new OkObjectResult(distance);

            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - Put(distance,id) method!"), exception);
            }
        }

        /// <summary>
        /// The GET method requests a representation of the specified resource matching the provided search criteria.
        /// In our case the provided search criteria is the start city and the stop city.
        /// </summary>
        /// /// <remarks>
        /// GET method will return to user a specific road distance that will match the criteria <b>startCity</b> and <b>stopCity</b>.
        /// </remarks>
        /// <param name="startCity">The city from which to start.</param>
        /// <param name="destinationCity">The city we have to reach.</param>
        [HttpGet("{startCity}/{destinationCity}")]
        public async Task<IActionResult> GetRoadDistance(string startCity, string destinationCity)
        {
            try
            {
                var getDistance = await _distancesLogic.GetRoadDistanceAsync(startCity, destinationCity);
                return new OkObjectResult(getDistance);

            }
            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in DistancesController - GetRoadDistance(startCity,destinationCity) method!"), exception);
            }
        }
        #endregion

    }
}
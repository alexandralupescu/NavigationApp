/**************************************************************************
 *                                                                        *
 *  File:        IDAStarController.cs                                     *
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navigation.AStar.Implementations;
using Navigation.Business.Logic.Interfaces;

namespace Navigation.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IDAStarController : ControllerBase
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        private readonly IDAStarAlgorithm _idaStar;
        #endregion

        #region Constructors
        /// <summary>
        /// IDAStarController constructor.
        /// </summary>
        /// <remarks>
        /// IDAStarController has a dependency on ICitiesLogic because it delegates some responsabilities 
        /// to CitiesLogic class, has a dependency on IDistancesLogic because it delegates some responsabilities 
        /// to DistancesLogic class and has also a dependency on IDAStarAlgorithm class.
        /// </remarks>
        /// <param name="citiesLogic">Used to access the data from the Business layer.</param>
        /// <param name="distancesLogic">Used to access the data from the Business layer.</param>
        public IDAStarController(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
            _idaStar = new IDAStarAlgorithm(_citiesLogic,_distancesLogic);
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET method will return the result of IDA* pathfinding search algorithm.
        /// </summary>
        /// <remarks>
        /// The GET method requests a representation of the specified resource.
        /// </remarks>
        [HttpGet("{startCity}/{destinationCity}")]
        public async Task<IActionResult> GetIDAStarResult(string startCity, string destinationCity)
        {
            try
            {
                List<string> list = await _idaStar.ResolveIDAStarAlgorithm(startCity, destinationCity);
                return new OkObjectResult(list);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in IDAStarController - GetIDAStarResultAsync(startCity,destinationCity) method!"), exception);
            }

        }
        #endregion
    }
}
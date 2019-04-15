﻿/**************************************************************************
 *                                                                        *
 *  File:        AStarController.cs                                         *
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
    /// <summary>
    /// AStarController is responsible for controlling the algorithm flow execution.
    /// </summary>
    /// <remarks>
    /// The Controller is responsible for controlling the application logic and acts as the 
    /// coordinator between the View and the Model. The Controller receives an input from 
    /// the users via the View, then processes the user's data with the help of Model and 
    /// passes the results back to the View.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class AStarController : Controller
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        private readonly AStarAlgorithm _aStar;
        #endregion

        #region Constructors
        /// <summary>
        /// AStarController constructor.
        /// </summary>
        /// <remarks>
        /// AStarController has a dependency on ICitiesLogic because it delegates some responsabilities 
        /// to CitiesLogic class, has a dependency on IDistancesLogic because it delegates some responsabilities 
        /// to DistancesLogic class and has also a dependency on AStarAlgorithm class.
        /// </remarks>
        /// <param name="citiesLogic">Used to access the data from the Business layer.</param>
        /// <param name="distancesLogic">Used to access the data from the Business layer.</param>
        public AStarController(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
            _aStar = new AStarAlgorithm(_citiesLogic, _distancesLogic);
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET method will return the result of A* pathfinding search algorithm.
        /// </summary>
        /// <remarks>
        /// The GET method requests a representation of the specified resource.
        /// </remarks>
        [HttpGet("{startCity}/{destinationCity}")]
        public async Task<IActionResult> GetAStarResult(string startCity, string destinationCity)
        {
            try
            {
                List<string> list = await _aStar.ResolveAlgorithm(startCity, destinationCity);
                return new OkObjectResult(list);
            }
            catch(Exception exception)
            {
                throw new Exception(
                  string.Format("Error in AStarController - GetAStarResultAsync(startCity,destinationCity) method!"), exception);
            }
            
        }

        /// <summary>
        /// GET method will return the result of A* pathfinding search algorithm with an intermediate city.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="intermediateCity">An intermediate city that will be chosen by the user.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        [HttpGet("{startCity}/{intermediateCity}/{destinationCity}")]
        public async Task<IActionResult> GetAStarWithIntermediateResult(string startCity, string intermediateCity, string destinationCity)
        {
            try
            {
                List<string> list = await _aStar.ResolveAlgorithmIntermediateProblem(startCity, intermediateCity, destinationCity);
                return new OkObjectResult(list);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in AStarController - GetAStarWithIntermediateResultAsync(startCity,intermediareCity,destinationCity) method!"), exception);
            }

        }
        #endregion
    }
}
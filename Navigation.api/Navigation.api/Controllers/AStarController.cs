﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="citiesLogic">Used to access the data from the Business layer</param>
        /// <param name="distancesLogic">Used to access the data from the Business layer</param>
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
        [HttpGet("startCity={startCity}/destinationCity={destinationCity}")]
        public async Task<IActionResult> GetAStarResultAsync(string startCity, string destinationCity)
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
        #endregion
    }
}
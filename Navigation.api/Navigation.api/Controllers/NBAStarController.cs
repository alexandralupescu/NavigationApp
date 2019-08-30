/**************************************************************************
 *                                                                        *
 *  File:        NBAStarController.cs                                     *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Navigation.Algorithm.Algorithms;
using Navigation.Business.Logic.Interfaces;

namespace Navigation.api.Controllers
{
    /// <summary>
    /// NBAStarController is responsible for controlling the algorithm flow execution.
    /// </summary>
    /// <remarks>
    /// The Controller is responsible for controlling the application logic and acts as the 
    /// coordinator between the View and the Model. The Controller receives an input from 
    /// the users via the View, then processes the user's data with the help of Model and 
    /// passes the results back to the View.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class NBAStarController : ControllerBase
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        private readonly NBAStar _nbaStar;
        #endregion

        #region Constructor
        /// <summary>
        /// NBAStarController constructor.
        /// </summary>
        /// <remarks>
        /// AStarController has a dependency on ICitiesLogic because it delegates some responsabilities 
        /// to CitiesLogic class, has a dependency on IDistancesLogic because it delegates some responsabilities 
        /// to DistancesLogic class and has also a dependency on NBAStar class.
        /// </remarks>
        /// <param name="citiesLogic">Used to access the data from the Business layer.</param>
        /// <param name="distancesLogic">Used to access the data from the Business layer.</param>
        public NBAStarController(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
            _nbaStar = new NBAStar(_citiesLogic, _distancesLogic);
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET method will return the result of NBA* pathfinding search algorithm with or without an intermediate city.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        [HttpGet()]
        public async Task<IActionResult> GetNBAStarResult(string startCity, [FromQuery]List<string> destinationCity)
        {
            try
            {
                /* Calculate algorithm execution time. */
                long watch = Stopwatch.GetTimestamp();
                Dictionary<string, List<string>> dict = await _nbaStar.GetNBAStarAsync(startCity, destinationCity);
                double dif = (Stopwatch.GetTimestamp() - watch) * 1000.0 / Stopwatch.Frequency;

                Dictionary<string, List<string>> finalDict = new Dictionary<string, List<string>>();
                Random rnd = new Random();
                for (int i = 0; i < dict.Count - 1; i = i + 1)
                {
                    finalDict.Add(rnd.Next(1, 100) + "." + dict.ElementAt(i).Key, new List<string> { String.Join(",", dict.ElementAt(i).Value) });

                }

                finalDict.Add("finalCost", new List<string> { (Double.Parse(String.Join("", dict.ElementAt(dict.Count - 1).Value))).ToString() });

                return new OkObjectResult(dict);
            }
            catch (Exception exception)
            {
                throw new Exception(
                  string.Format("Error in NBAStarController - GetNBAStarResult method!"), exception);
            }

        }
        #endregion
    }


}
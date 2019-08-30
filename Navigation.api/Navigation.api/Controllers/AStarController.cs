/**************************************************************************
 *                                                                        *
 *  File:        AStarController.cs                                       *
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
        private readonly AStar _aStar;
        #endregion

        #region Constructor
        /// <summary>
        /// AStarController constructor.
        /// </summary>
        /// <remarks>
        /// AStarController has a dependency on ICitiesLogic because it delegates some responsabilities 
        /// to CitiesLogic class, has a dependency on IDistancesLogic because it delegates some responsabilities 
        /// to DistancesLogic class and has also a dependency on AStar class.
        /// </remarks>
        /// <param name="citiesLogic">Used to access the data from the Business layer.</param>
        /// <param name="distancesLogic">Used to access the data from the Business layer.</param>
        public AStarController(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
            _aStar = new AStar(_citiesLogic, _distancesLogic);
        }
        #endregion

        #region CRUD Methods
        /// <summary>
        /// GET method will return the result of A* pathfinding search algorithm with or without an intermediate city.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        [HttpGet()]
        public async Task<IActionResult> GetAStarResult(string startCity, [FromQuery]List<string> destinationCity)
        {

            if (!destinationCity.Any())
            {
                return new BadRequestObjectResult("Please enter valid values for start and destination city");
            }
            else
            {
                /* Calculate algorithm execution time. */
                long watch = Stopwatch.GetTimestamp();
                List<string> list = await _aStar.GetAStarAsync(startCity, destinationCity);
                double dif = (Stopwatch.GetTimestamp() - watch) * 1000.0 / Stopwatch.Frequency;

                Dictionary<string, List<string>> waypoints = new Dictionary<string, List<string>>();

                try
                {

                    if (destinationCity.Count() == 1)
                    {
                        List<string> noDupes = list.Distinct().ToList();

                        for (int i = 0; i < noDupes.Count - 1; i = i + 3)
                        {
                            waypoints.Add(i + "." + list[i], new List<string> { list[i + 1], list[i + 2] });
                        }

                        waypoints.Add("finalCost", new List<string> { (Double.Parse(list[list.Count - 1])).ToString() });
                    }
                    else
                    {
                        List<string> noDupes = list.ToList();

                        for (int i = 0; i < noDupes.Count - 1; i = i + 3)
                        {

                            waypoints.Add(i + "." + list[i], new List<string> { list[i + 1], list[i + 2] });
                        }

                        waypoints.Add("finalCost", new List<string> { (Double.Parse(list[list.Count - 1])).ToString() });
                    }

                    return new OkObjectResult(waypoints);
                }
                catch (Exception exception)
                {
                    throw new Exception(
                      string.Format("Error in AStarController - GetAStarResult method!"), exception);
                }
            }
        }
        #endregion
    }
}
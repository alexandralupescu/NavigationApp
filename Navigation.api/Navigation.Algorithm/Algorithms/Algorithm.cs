/**************************************************************************
 *                                                                        *
 *  File:        Algorithm.cs                                             *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using Navigation.Algorithm.Implementations;
using Navigation.Business.Logic.Interfaces;
using Navigation.DataAccess.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Navigation.Algorithm.Algorithms
{
    /// <summary>
    /// Generic Algorithm class.
    /// </summary>
    public class Algorithm
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        #endregion

        #region Constructors
        /// <summary>
        /// Algorithm constructor.
        /// </summary>
        /// <param name="citiesLogic">Access CitiesLogic methods from Business tier.</param>
        /// <param name="distancesLogic">Access DistancesLogic methods from Business tier.</param>
        public Algorithm(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method that is responsible for creating the graph. 
        /// </summary>
        /// <param name="graph">Instance of Graph class.</param>
        /// <returns></returns>
        public async virtual Task CreateGraph(Graph graph)
        {
            /* Fills a Graph with Romania map information. */
            /* The Graph contains Nodes that represents cities of Romania. */
            IEnumerable<Cities> cities = await _citiesLogic.GetAllCitiesAsync();
            foreach (Cities cityVar in cities)
            {
                Node cityNode = new Node(cityVar.CityName, null, cityVar.Latitude, cityVar.Longitude);
                graph.AddNode(cityNode);
            }

            /* Nodes are vertexes in the Graph. Connections between Nodes are edges. */
            /* Fills a Graph with edges between cities. */
            IEnumerable<Distances> distances = await _distancesLogic.GetAllDistancesAsync();
            foreach (Distances distanceVar in distances)
            {
                graph.AddUndirectedEdge(distanceVar.StartCity, distanceVar.DestinationCity, distanceVar.Distance);
            }
        }
        #endregion
    }
}

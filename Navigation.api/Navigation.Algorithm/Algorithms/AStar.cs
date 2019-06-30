﻿/**************************************************************************
 *                                                                        *
 *  File:        AStar.cs                                                 *
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
using Navigation.Algorithm.Implementations;
using Navigation.Algorithm.Interfaces;
using Navigation.Business.Logic.Interfaces;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Navigation.Algorithm.Algorithms
{
    /// <summary>
    /// AStar represents the main class that will call the necessary methods to 
    /// run the A* pathfinding search algorithm.
    /// </summary>
    public class AStar
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;
        #endregion

        #region Constructor
        /// <summary>
        /// AStarAlgorithm constructor.
        /// </summary>
        /// <param name="citiesLogic"></param>
        /// <param name="distancesLogic"></param>
        public AStar(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Method that is responsible for creating the graph. 
        /// </summary>
        /// <param name="graph"></param>
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

        /// <summary>
        /// Method that is responsible for assembling the A* pathfinding algorithm.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        /// <returns></returns>
        public async Task<List<string>> ResolveAlgorithm(string startCity, string destinationCity)
        {
            /* Creating the graph. */
            Graph graph = new Graph();
            await CreateGraph(graph);

            /* Store the start Node */
            Node start = graph.Nodes[startCity];
            /* Store the destination Node */
            Node destination = graph.Nodes[destinationCity];


            /* Function which tells us the exact distance between two neighbours. */
            double distance(Node node1, Node node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

            /* Estimation/Heuristic function (Haversine distance)
             * It tells us the estimated distance between the last node on a proposed path
             * and the destination node.  */
            double haversineEstimation(Node n) => Haversine.Distance(n, destination);

            Path<Node> shortestPath = FindPath(start, destination, distance, haversineEstimation);

            List<string> targetList = DisplayPath(shortestPath);

            return targetList;

        }

        /// <summary>
        /// Method that is responsible for display the final route and total cost.
        /// </summary>
        /// <param name="shorthestPath"></param>
        /// <returns></returns>
        public List<string> DisplayPath(Path<Node> shorthestPath)
        {
            /* Return the final result (the road and the distance based on start Node and destination Node). */
            List<string> finalList = new List<string>();
            double totalFinalCost = 0;
            foreach (Path<Node> path in shorthestPath.Reverse())
            {
                if (path.PreviousSteps != null)
                {
                    if (!finalList.Contains(path.PreviousSteps.LastStep.Key) ||
                        !finalList.Contains(path.LastStep.Key)){

                        finalList.Add(path.PreviousSteps.LastStep.Key);
                        finalList.Add(path.PreviousSteps.LastStep.Latitude.ToString());
                        finalList.Add(path.PreviousSteps.LastStep.Longitude.ToString());
                        finalList.Add(path.LastStep.Key);
                        finalList.Add(path.LastStep.Latitude.ToString());
                        finalList.Add(path.LastStep.Longitude.ToString());
                        totalFinalCost = path.TotalCost * 0.98;
                    }
                }

            }

            /* Eliminate duplicates. */
            List<string> noDups = finalList.Distinct().ToList();
            noDups.Add(totalFinalCost.ToString());
            return noDups;
        }

        /// <summary>
        /// Method that will be responsible to resolve A* pathfinding search with an intermediate city.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="intermediateCity">An intermediate city that will be chosen by the user.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        /// <returns></returns>
        public async Task<List<string>> GetAStarAsync(string startCity, List<string> destinationCity)
        {

            List<string> targetList = new List<string>();
            List<string> intermediateList = new List<string>();

            int counter = 0;
            double totalCost = 0;

            try
            {
                if (destinationCity.Count() == 1)
                {
                    targetList = await ResolveAlgorithm(startCity, destinationCity.First());
                    return targetList;

                }
                else
                {
                    targetList = await ResolveAlgorithm(startCity, destinationCity.First());
                    totalCost = Double.Parse(targetList.Last());
                    targetList.Remove(targetList.Last());

                    for (int index = 0; index < destinationCity.Count - 1; ++index)
                    {
                        counter = index;

                        intermediateList = await ResolveAlgorithm(destinationCity[counter], destinationCity[counter + 1]);
                        totalCost += Double.Parse(intermediateList.Last());
                        intermediateList.Remove(intermediateList.Last());

                        targetList = targetList.Concat(intermediateList).ToList();
                    }
                    
                    targetList.Add(totalCost.ToString());

                    return targetList;
                }
            }

            catch(Exception exception)
            {
                throw new Exception(
                   string.Format("Error in AStar class - GetAStarAsync method!"), exception);
            }

        }

        /// <summary>
        /// This is the method responsible for finding the shortest path between a start and destination cities 
        /// using the A* pathfinding search algorithm.
        /// </summary>
        /// <typeparam name="TNode">The Node type instance</typeparam>
        /// <param name="start">Start city</param>
        /// <param name="destination">Destination city</param>
        /// <param name="distance">Function which tells us the exact distance between two neighbours.</param>
        /// <param name="estimate">Function which tells us the estimated distance between the last node on a proposed path and the
        /// destination node.</param>
        public Path<TNode> FindPath<TNode>(
            TNode start,
            TNode destination,
            Func<TNode, TNode, double> distance,
            Func<TNode, double> estimate) where TNode : IHasNeighbours<TNode>
        {
            var closed = new HashSet<TNode>();

            var queue = new PriorityQueue<double, Path<TNode>>();

            queue.Enqueue(0, new Path<TNode>(start));

            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();

                if (closed.Contains(path.LastStep))
                {
                    continue;
                }


                if (path.LastStep.Equals(destination))
                {
                    return path;
                }

                closed.Add(path.LastStep);

                foreach (TNode n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);

                    var newPath = path.AddStep(n, d);

                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }

            return null;
        }

        #endregion

    }



}

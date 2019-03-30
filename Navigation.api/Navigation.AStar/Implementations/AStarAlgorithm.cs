using MongoDB.Bson.IO;
using Navigation.Business.Logic.Interfaces;
using Navigation.Business.Models;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.AStar.Implementations
{
    /// <summary>
    /// AStarAlgorithm represents the main class that will call the necessary methods to 
    /// run the A* pathfinding search algorithm.
    /// </summary>
    public class AStarAlgorithm
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
        /// AStarAlgorithm constructor.
        /// </summary>
        /// <param name="citiesLogic"></param>
        /// <param name="distancesLogic"></param>
        public AStarAlgorithm(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;
        }
        #endregion

        #region Public Methods
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

            /* Fills a Graph with Romania map information. */
            /* The Graph contains Nodes that represents Cities of Romania. */
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
            double haversineEstimation(Node n) => Haversine.Distance(n, destination, DistanceType.km); 

            Path<Node> shortestPath = FindPath(start, destination, distance, haversineEstimation);

            /* Return the final result (the road and the distance based on start Node and destination Node) */
            List<string> list = new List<string>();
            foreach(Path<Node> path in shortestPath.Reverse())
            {
                if (path.PreviousSteps != null)
                {
                    list.Add(path.PreviousSteps.LastStep.Key);
                    list.Add(path.LastStep.Key);
                    list.Add(path.TotalCost.ToString());

                }

            }

            return list;

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

    #region Partial Node class
    sealed partial class Node : IHasNeighbours<Node>
    {
        public IEnumerable<Node> Neighbours
        {
            get
            {
                List<Node> nodes = new List<Node>();

                foreach (EdgeToNeighbor etn in Neighbors)
                {
                    nodes.Add(etn.Neighbor);
                }

                return nodes;
            }
        }
    }

    #endregion


}

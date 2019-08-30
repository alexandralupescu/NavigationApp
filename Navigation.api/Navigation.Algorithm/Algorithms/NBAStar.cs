/**************************************************************************
 *                                                                        *
 *  File:        NBAStar.cs                                               *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using Navigation.Algorithm.Implementations;
using Navigation.Business.Logic.Interfaces;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Navigation.Algorithm.Algorithms
{
    /// <summary>
    /// NBAStar represents the main class that will call the necessary methods
    /// to run the NBA* algorithm.
    /// </summary>
    public class NBAStar : Algorithm
    {
        #region Private Members
        /// <summary>
        /// Queues that stores the cities by priority (f cost) for the two processes.
        /// </summary>
        private PriorityQueue<double, Path<Node>> _openProcessNo_1;
        private PriorityQueue<double, Path<Node>> _openProcessNo_2;

        /// <summary>
        /// Dictionaries that stores the neighbours for a current node.
        /// </summary>
        private Dictionary<Node, Node> _parentsProcessNo_1;
        private Dictionary<Node, Node> _parentsProcessNo_2;

        /// <summary>
        /// Dictionaries that stores the distances for the both processes.
        /// </summary>
        private Dictionary<Node, double> _distanceProcessNo_1;
        private Dictionary<Node, double> _distanceProcessNo_2;

        /// <summary>
        /// List that will store the nodes that have been traversed.
        /// </summary>
        private List<string> _closed;

        /// <summary>
        /// Intersection node.
        /// </summary>
        private Node _touchNode;

        /// <summary>
        /// F cost parameter for the first process.
        /// </summary>
        private double _fProcessNo_1;

        /// <summary>
        /// F cost parameter for the second process.
        /// </summary>
        private double _fProcessNo_2;

        /// <summary>
        /// Parameter that will store the best score found by the algorithm.
        /// </summary>
        private double _bestPathLength;
        #endregion

        #region Constructors
        /// <summary>
        /// NBAStar constructor.
        /// </summary>
        /// <param name="citiesLogic">Access CitiesLogic methods from Business tier.</param>
        /// <param name="distancesLogic">Access DistancesLogic methods from Business tier.</param>
        public NBAStar(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic) : base(citiesLogic,distancesLogic)
        {

            _openProcessNo_1 = new PriorityQueue<double, Path<Node>>();
            _openProcessNo_2 = new PriorityQueue<double, Path<Node>>();

            _parentsProcessNo_1 = new Dictionary<Node, Node>();
            _parentsProcessNo_2 = new Dictionary<Node, Node>();

            _distanceProcessNo_1 = new Dictionary<Node, double>();
            _distanceProcessNo_2 = new Dictionary<Node, double>();

            _closed = new List<string>();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize all the variables neccessary in the process of the algorithm.
        /// </summary>
        /// <param name="startCity"></param>
        /// <param name="destinationCity"></param>
        private void Init(Node startCity, Node destinationCity)
        {
            /* Clear step. */
            _openProcessNo_1.Clear();
            _openProcessNo_2.Clear();

            _distanceProcessNo_1.Clear();
            _distanceProcessNo_2.Clear();

            _parentsProcessNo_1.Clear();
            _parentsProcessNo_2.Clear();

            _closed.Clear();

            double haversineEstimation(Node n) => Haversine.Distance(n, destinationCity);

            double totalDistance = haversineEstimation(startCity);

            _fProcessNo_1 = totalDistance;
            _fProcessNo_2 = totalDistance;

            _bestPathLength = double.MaxValue;
            _touchNode = null;

            _openProcessNo_1.Enqueue(_fProcessNo_1, new Path<Node>(startCity));
            _openProcessNo_2.Enqueue(_fProcessNo_2, new Path<Node>(destinationCity));

            _parentsProcessNo_1.Add(startCity, null);
            _parentsProcessNo_2.Add(destinationCity, null);

            _distanceProcessNo_1.Add(startCity, 0.0);
            _distanceProcessNo_2.Add(destinationCity, 0.0);

        }

        /// <summary>
        /// TraceBack method is used to display the final path.
        /// </summary>
        /// <param name="touchNode">touchNode represents intersection node (common node).</param>
        /// <param name="parentsProcessNo_1">parentsProcessNo_1 represents the Dictionary for one part of the final route.</param>
        /// <param name="parentsProcessNo_2">parentsProcessNo_2 represents the Dictionary for the other part of the final route.</param>
        /// <param name="totalDistance">totalDistance represents total number of km.</param>
        /// <returns></returns>
        private Dictionary<string, List<string>> TraceBack(Node touchNode, Dictionary<Node, Node> parentsProcessNo_1, Dictionary<Node, Node> parentsProcessNo_2, double totalDistance)
        {

            Dictionary<string, List<string>> path = new Dictionary<string, List<string>>();
            Node currentNode = touchNode;

            int counter = 0;
            Random rnd = new Random();

            while (currentNode != null)
            {
                path.Add(rnd.Next(1,100) + "."  + currentNode.Key, new List<string> { currentNode.Latitude.ToString(), currentNode.Longitude.ToString() });
                currentNode = parentsProcessNo_1[currentNode];
                counter++;

            }

            var reversedPath = path.Reverse().ToDictionary(x => x.Key, x => x.Value);

            if (parentsProcessNo_2 != null)
            {
                currentNode = parentsProcessNo_2[touchNode];

                while (currentNode != null)
                {

                    reversedPath.Add(rnd.Next(1,100) + "." + currentNode.Key, new List<string> { currentNode.Latitude.ToString(), currentNode.Longitude.ToString() });
                    currentNode = parentsProcessNo_2[currentNode];
                    counter++;
                }


            }

            reversedPath.Add("finalCost", new List<string> { totalDistance.ToString() });

            return reversedPath;
        }

        /// <summary>
        /// ExpandInForwardDirection represents the first process.
        /// The search will start with the initial start city and the scope
        /// will be the initial destination.
        /// </summary>
        /// <param name="start">Name of the city, the user will start the search.</param>
        /// <param name="destination">Name of the city, the user will want to reach.</param>
        private void ExpandInForwardDirection(Node start, Node destination)
        {

            var currentNodePath = _openProcessNo_1.Dequeue();


            /* Function which tells us the exact distance between two neighbours. */
            double distance(Node node1, Node node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

            if (_closed.Contains(currentNodePath.LastStep.Key))
            {
                return;
            }

            _closed.Add(currentNodePath.LastStep.Key);


            Path<Node> path = new Path<Node>(currentNodePath.LastStep);
            double fCost = 0;

            if ((_distanceProcessNo_1[currentNodePath.LastStep] + Haversine.Distance(currentNodePath.LastStep, destination)) >= _bestPathLength ||
                (_distanceProcessNo_1[currentNodePath.LastStep] + _fProcessNo_2 - Haversine.Distance(currentNodePath.LastStep, start) >= _bestPathLength))
            {
                // reject the current node
            }
            else
            {
                foreach (Node childNode in currentNodePath.LastStep.Neighbours)
                {
                    if (_closed.Contains(childNode.Key))
                    {
                        continue;
                    }

                    double tentativeDistance = _distanceProcessNo_1[currentNodePath.LastStep] + distance(currentNodePath.LastStep, childNode);


                    if (!_distanceProcessNo_1.ContainsKey(childNode) || (_distanceProcessNo_1[childNode] > tentativeDistance))
                    {
                        if (_distanceProcessNo_1.ContainsKey(childNode))
                        {
                            _distanceProcessNo_1.Remove(childNode);
                        }

                        if (_parentsProcessNo_1.ContainsKey(childNode))
                        {
                            _parentsProcessNo_1.Remove(childNode);
                        }

                        _distanceProcessNo_1.Add(childNode, tentativeDistance);
                        _parentsProcessNo_1.Add(childNode, currentNodePath.LastStep);
                        _openProcessNo_1.Enqueue(tentativeDistance + Haversine.Distance(childNode, destination), new Path<Node>(childNode));
                        fCost = tentativeDistance + Haversine.Distance(childNode, destination);


                        if (_distanceProcessNo_2.ContainsKey(childNode))
                        {
                            double pathLength = tentativeDistance + _distanceProcessNo_2[childNode];


                            if (_bestPathLength > pathLength)
                            {
                                _bestPathLength = pathLength;
                                _touchNode = childNode;
                            }

                        }

                    }

                }

            }

            if (!_openProcessNo_1.IsEmpty)
            {
                _fProcessNo_1 = _openProcessNo_1.Peek;

            }
        }

        /// <summary>
        /// ExpandInBackwardDirection is the inversed process. 
        /// The initial destination will be the start node and the initial start city
        /// will be destination.
        /// </summary>
        /// <param name="start">Name of the city, the user will start the search.</param>
        /// <param name="destination">Name of the city, the user will want to reach.</param>
        private void ExpandInBackwardDirection(Node start, Node destination)
        {
            var currentNodePath = _openProcessNo_2.Dequeue();


            /* Function which tells us the exact distance between two neighbours. */
            double distance(Node node1, Node node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

            if (_closed.Contains(currentNodePath.LastStep.Key))
            {
                return;
            }


            Path<Node> path = new Path<Node>(currentNodePath.LastStep);
            double fCost = 0;

            if ((_distanceProcessNo_2[currentNodePath.LastStep] + Haversine.Distance(currentNodePath.LastStep, start)) >= _bestPathLength ||
                (_distanceProcessNo_2[currentNodePath.LastStep] + _fProcessNo_1 - Haversine.Distance(currentNodePath.LastStep, destination) >= _bestPathLength))
            {
                // reject the current node
            }
            else
            {
                foreach (Node parentNode in path.LastStep.Neighbours)
                {
                    if (_closed.Contains(parentNode.Key))
                    {
                        continue;
                    }

                    double tentativeDistance = _distanceProcessNo_2[currentNodePath.LastStep] + distance(parentNode, currentNodePath.LastStep);


                    if (!_distanceProcessNo_2.ContainsKey(parentNode) || (_distanceProcessNo_2[parentNode] > tentativeDistance))
                    {
                        if (_distanceProcessNo_2.ContainsKey(parentNode))
                        {
                            _distanceProcessNo_2.Remove(parentNode);
                        }
                        if (_parentsProcessNo_2.ContainsKey(parentNode))
                        {
                            _parentsProcessNo_2.Remove(parentNode);
                        }
                        _distanceProcessNo_2.Add(parentNode, tentativeDistance);
                        _parentsProcessNo_2.Add(parentNode, currentNodePath.LastStep);
                        _openProcessNo_2.Enqueue(tentativeDistance + Haversine.Distance(parentNode, destination), new Path<Node>(parentNode));
                        fCost = tentativeDistance + Haversine.Distance(parentNode, destination);


                        if (_distanceProcessNo_1.ContainsKey(parentNode))
                        {
                            double pathLength = tentativeDistance + _distanceProcessNo_1[parentNode];


                            if (_bestPathLength > pathLength)
                            {

                                _bestPathLength = pathLength;
                                _touchNode = parentNode;
                            }


                        }

                    }


                }


            }

            if (!_openProcessNo_2.IsEmpty)
            {
                _fProcessNo_2 = _openProcessNo_2.Peek;

            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Main loop of the algorithm.
        /// </summary>
        /// <param name="startCity">Name of the city, the user will start the search.</param>
        /// <param name="destinationCity">Name of the city, the user will want to reach.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>> SearchNBAAsync(string startCity, string destinationCity)
        {

            /* Creating the graph. */
            Graph graph = new Graph();
            await CreateGraph(graph);

            /* Store the start Node */
            Node start = graph.Nodes[startCity];
            /* Store the destination Node */
            Node destination = graph.Nodes[destinationCity];

            /* Initialize the necessary variables. */
            Init(start, destination);

            Dictionary<string, List<string>> finalDict = new Dictionary<string, List<string>>();

            if (start.Key.Equals(destination.Key))
            {
                finalDict.Add(start.Key, new List<string> { start.Latitude.ToString(), start.Longitude.ToString() });
                return finalDict;
            }

            while (!_openProcessNo_1.IsEmpty && !_openProcessNo_2.IsEmpty)
            {
                if (_openProcessNo_1.Count < _openProcessNo_2.Count)
                {
                    ExpandInForwardDirection(start, destination);
                }
                else
                {
                    ExpandInBackwardDirection(start, destination);
                }

            }


            if (_touchNode == null)
            {
                return finalDict;
            }


            return TraceBack(_touchNode, _parentsProcessNo_1, _parentsProcessNo_2, _bestPathLength * 1.01);
        }

        /// <summary>
        /// GetNBAStarAsync method treats the cases: two inputs or three inputs.
        /// The result will be a dictionary with the final result and the total distance measured in km.
        /// </summary>
        /// <param name="startCity">Name of the city, the user will start the search.</param>
        /// <param name="destinationCity">Name of the city, the user will want to reach.</param>
        /// <returns></returns>
        public async Task<Dictionary<string, List<string>>> GetNBAStarAsync(string startCity, List<string> destinationCity)
        {

            Dictionary<string, List<string>> targetDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> intermediateDict = new Dictionary<string, List<string>>();

            int counter = 0;
            double totalCost = 0;


            try
            {
                if (destinationCity.Count() == 1)
                {
                    targetDict = await SearchNBAAsync(startCity, destinationCity.First());
                    return targetDict;

                }
                else
                {
                    targetDict = await SearchNBAAsync(startCity, destinationCity.First());
                    totalCost = _bestPathLength;
                    targetDict.Remove(targetDict.Keys.Last());

                    for (int index = 0; index < destinationCity.Count - 1; ++index)
                    {
                        counter = index;

                        intermediateDict = await SearchNBAAsync(destinationCity[counter], destinationCity[counter + 1]);
                        totalCost += (_bestPathLength);

                        targetDict = targetDict.Concat(intermediateDict).ToDictionary(x => x.Key, x => x.Value);
                        targetDict.Remove(targetDict.Keys.Last());
                    }

                    targetDict.Add("finalCost", new List<string> { (totalCost * 1.01).ToString() });
                    return targetDict;
                }

            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in GetAStarAsync - GetNBAStarAsync method!"), exception);
            }

        }
        #endregion

    }
}

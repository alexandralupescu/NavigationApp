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
    public class NBAStar
    {
        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;

        /// <summary>
        /// Queues that stores the cities by priority (f cost) for
        /// the two processes.
        /// </summary>
        private PriorityQueue<double, Path<Node>> openProcessNo_1;
        private PriorityQueue<double, Path<Node>> openProcessNo_2;
        /// <summary>
        /// Dictionaries that stores the neighbours for a current node.
        /// </summary>
        private Dictionary<Node, Node> parentsProcessNo_1;
        private Dictionary<Node, Node> parentsProcessNo_2;
        /// <summary>
        /// Dictionaries that stores the distances for the both processes.
        /// </summary>
        private Dictionary<Node, double> distanceProcessNo_1;
        private Dictionary<Node, double> distanceProcessNo_2;
        /// <summary>
        /// List will store the nodes that have been traversed.
        /// </summary>
        private List<string> closed;
        /// <summary>
        /// Intersection node.
        /// </summary>
        private Node touchNode;

        /// <summary>
        /// F cost parameter for the first process.
        /// </summary>
        private double fProcessNo_1;
        /// <summary>
        /// F cost parameter for the second process.
        /// </summary>
        private double fProcessNo_2;
        /// <summary>
        /// Parameter that will store the best score has found the algorithm.
        /// </summary>
        private double bestPathLength;
        #endregion

        #region Constructor
        /// <summary>
        /// NBA* constructor.
        /// </summary>
        /// <param name="citiesLogic"></param>
        /// <param name="distancesLogic"></param>
        public NBAStar(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;


            openProcessNo_1 = new PriorityQueue<double, Path<Node>>();
            openProcessNo_2 = new PriorityQueue<double, Path<Node>>();

            parentsProcessNo_1 = new Dictionary<Node, Node>();
            parentsProcessNo_2 = new Dictionary<Node, Node>();

            distanceProcessNo_1 = new Dictionary<Node, double>();
            distanceProcessNo_2 = new Dictionary<Node, double>();

            closed = new List<string>();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize all the variables neccessary in process the algorithm.
        /// </summary>
        /// <param name="startCity"></param>
        /// <param name="destinationCity"></param>
        private void Init(Node startCity, Node destinationCity)
        {
            openProcessNo_1.Clear();
            openProcessNo_2.Clear();

            distanceProcessNo_1.Clear();
            distanceProcessNo_2.Clear();

            parentsProcessNo_1.Clear();
            parentsProcessNo_2.Clear();

            closed.Clear();

            double haversineEstimation(Node n) => Haversine.Distance(n, destinationCity);

            double totalDistance = haversineEstimation(startCity);

            fProcessNo_1 = totalDistance;
            fProcessNo_2 = totalDistance;

            bestPathLength = double.MaxValue;
            touchNode = null;

            openProcessNo_1.Enqueue(fProcessNo_1, new Path<Node>(startCity));
            openProcessNo_2.Enqueue(fProcessNo_2, new Path<Node>(destinationCity));

            parentsProcessNo_1.Add(startCity, null);
            parentsProcessNo_2.Add(destinationCity, null);

            distanceProcessNo_1.Add(startCity, 0.0);
            distanceProcessNo_2.Add(destinationCity, 0.0);

        }

        private Dictionary<string, List<string>> TraceBack(Node touchNode, Dictionary<Node, Node> parentsProcessNo_1, Dictionary<Node, Node> parentsProcessNo_2, double totalDistance)
        {

            Dictionary<string, List<string>> path = new Dictionary<string, List<string>>();
            Node currentNode = touchNode;

            while (currentNode != null)
            {
                path.Add(currentNode.Key, new List<string> { currentNode.Latitude.ToString(), currentNode.Longitude.ToString() });
                currentNode = parentsProcessNo_1[currentNode];

            }

            var reversedPath = path.Reverse().ToDictionary(x => x.Key, x => x.Value);

            if (parentsProcessNo_2 != null)
            {
                currentNode = parentsProcessNo_2[touchNode];

                while (currentNode != null)
                {

                    reversedPath.Add(currentNode.Key, new List<string> { currentNode.Latitude.ToString(), currentNode.Longitude.ToString() });
                    currentNode = parentsProcessNo_2[currentNode];
                }


            }

            reversedPath.Add("totalDistance", new List<string> { totalDistance.ToString() });

            return reversedPath;
        }


        private void ExpandInForwardDirection(Node start, Node destination)
        {

            var currentNodePath = openProcessNo_1.Dequeue();


            /* Function which tells us the exact distance between two neighbours. */
            double distance(Node node1, Node node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

            if (closed.Contains(currentNodePath.LastStep.Key))
            {
                return;
            }

            closed.Add(currentNodePath.LastStep.Key);


            Path<Node> path = new Path<Node>(currentNodePath.LastStep);
            double fCost = 0;

            if ((distanceProcessNo_1[currentNodePath.LastStep] + Haversine.Distance(currentNodePath.LastStep, destination)) >= bestPathLength ||
                (distanceProcessNo_1[currentNodePath.LastStep] + fProcessNo_2 - Haversine.Distance(currentNodePath.LastStep, start) >= bestPathLength))
            {
                // reject the current node
            }
            else
            {
                foreach (Node childNode in currentNodePath.LastStep.Neighbours)
                {
                    if (closed.Contains(childNode.Key))
                    {
                        continue;
                    }

                    double tentativeDistance = distanceProcessNo_1[currentNodePath.LastStep] + distance(currentNodePath.LastStep, childNode);


                    if (!distanceProcessNo_1.ContainsKey(childNode) || (distanceProcessNo_1[childNode] > tentativeDistance))
                    {
                        distanceProcessNo_1.Add(childNode, tentativeDistance);
                        parentsProcessNo_1.Add(childNode, currentNodePath.LastStep);
                        openProcessNo_1.Enqueue(tentativeDistance + Haversine.Distance(childNode, destination), new Path<Node>(childNode));
                        fCost = tentativeDistance + Haversine.Distance(childNode, destination);


                        if (distanceProcessNo_2.ContainsKey(childNode))
                        {
                            double pathLength = tentativeDistance + distanceProcessNo_2[childNode];


                            if (bestPathLength > pathLength)
                            {
                                bestPathLength = pathLength;
                                touchNode = childNode;
                            }


                        }

                    }


                }


            }

            if (!openProcessNo_1.IsEmpty)
            {
                fProcessNo_1 = openProcessNo_1.Peek;

            }



        }


        private void ExpandInBackwardDirection(Node start, Node destination)
        {
            var currentNodePath = openProcessNo_2.Dequeue();


            /* Function which tells us the exact distance between two neighbours. */
            double distance(Node node1, Node node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

            if (closed.Contains(currentNodePath.LastStep.Key))
            {
                return;
            }


            Path<Node> path = new Path<Node>(currentNodePath.LastStep);
            double fCost = 0;

            if ((distanceProcessNo_2[currentNodePath.LastStep] + Haversine.Distance(currentNodePath.LastStep, start)) >= bestPathLength ||
                (distanceProcessNo_2[currentNodePath.LastStep] + fProcessNo_1 - Haversine.Distance(currentNodePath.LastStep, destination) >= bestPathLength))
            {
                // reject the current node
            }
            else
            {
                foreach (Node parentNode in path.LastStep.Neighbours)
                {
                    if (closed.Contains(parentNode.Key))
                    {
                        continue;
                    }

                    double tentativeDistance = distanceProcessNo_2[currentNodePath.LastStep] + distance(parentNode, currentNodePath.LastStep);


                    if (!distanceProcessNo_2.ContainsKey(parentNode) || (distanceProcessNo_2[parentNode] > tentativeDistance))
                    {
                        distanceProcessNo_2.Add(parentNode, tentativeDistance);
                        parentsProcessNo_2.Add(parentNode, currentNodePath.LastStep);
                        openProcessNo_2.Enqueue(tentativeDistance + Haversine.Distance(parentNode, destination), new Path<Node>(parentNode));
                        fCost = tentativeDistance + Haversine.Distance(parentNode, destination);


                        if (distanceProcessNo_1.ContainsKey(parentNode))
                        {
                            double pathLength = tentativeDistance + distanceProcessNo_1[parentNode];


                            if (bestPathLength > pathLength)
                            {

                                bestPathLength = pathLength;
                                touchNode = parentNode;
                            }


                        }

                    }


                }


            }



            if (!openProcessNo_2.IsEmpty)
            {
                fProcessNo_2 = openProcessNo_2.Peek;

            }
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
        }


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

            double distance = 0;


            while (!openProcessNo_1.IsEmpty && !openProcessNo_2.IsEmpty)
            {
                if (openProcessNo_1.Count < openProcessNo_2.Count)
                {
                    ExpandInForwardDirection(start, destination);
                }
                else
                {
                    ExpandInBackwardDirection(start, destination);
                }

            }


            if (touchNode == null)
            {
                return finalDict;
            }


            return TraceBack(touchNode, parentsProcessNo_1, parentsProcessNo_2, distance);
        }


        public async Task<Dictionary<string, List<string>>> GetNBAStarAsync(string startCity, List<string> destinationCity)
        {

            Dictionary<string, List<string>> targetDict = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> intermediateDict = new Dictionary<string, List<string>>();

            int counter = 0;


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

                    for (int index = 0; index < destinationCity.Count - 1; ++index)
                    {
                        counter = index;
                        intermediateDict = await SearchNBAAsync(destinationCity[counter], destinationCity[counter + 1]);

                        targetDict = targetDict.Concat(intermediateDict).ToDictionary(x => x.Key, x => x.Value);
                    }



                    return targetDict;
                }

            }

            catch (Exception exception)
            {
                throw new Exception(
                   string.Format("Error in GetAStarAsync - GetAStarBiAsync(destinationCity) method!"), exception);
            }

        }
        #endregion



    }
}

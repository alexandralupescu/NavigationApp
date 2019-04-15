﻿/**************************************************************************
 *                                                                        *
 *  File:        IDAStarAlgorithm.cs                                      *
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
using Navigation.Business.Logic.Interfaces;
using Navigation.DataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Navigation.AStar.Implementations
{
    /// <summary>
    /// IDAStarAlgorithm represents the main class that will call the necessary methods to 
    /// run the IDA* pathfinding search algorithm.
    /// </summary>
    public class IDAStarAlgorithm
    {

        #region Private Members
        /// <summary>
        /// Used to access the data from the Business layer.
        /// </summary>
        private readonly ICitiesLogic _citiesLogic;
        private readonly IDistancesLogic _distancesLogic;

        /// <summary>
        /// List of visited nodes.
        /// </summary>
        private static List<Node> _visitedNodes;
        /// <summary>
        /// Choose the minimum treshold.
        /// </summary>
        private static double _minValue;
        /// <summary>
        /// Set cutt-off limit or treshold for f() function.
        /// </summary>
        private static double _costLimit = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// IDAStarAlgorithm constructor.
        /// </summary>
        /// <param name="citiesLogic"></param>
        /// <param name="distancesLogic"></param>
        public IDAStarAlgorithm(ICitiesLogic citiesLogic, IDistancesLogic distancesLogic)
        {
            _citiesLogic = citiesLogic;
            _distancesLogic = distancesLogic;

            _visitedNodes = new List<Node>();
            _costLimit = _minValue = double.MaxValue;
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
        /// Method that is responsible for assembling the IDA* pathfinding algorithm.
        /// </summary>
        /// <param name="startCity">The city from which the user will start.</param>
        /// <param name="destinationCity">The city which user will arrive.</param>
        /// <returns></returns>
        public async Task<List<string>> ResolveIDAStarAlgorithm(string startCity, string destinationCity)
        {
            /* Creating the graph. */
            Graph graph = new Graph();
            await CreateGraph(graph);

            List<string> path = new List<string>();

            /* Store the start Node */
            Node start = graph.Nodes[startCity];
            /* Store the destination Node */
            Node destination = graph.Nodes[destinationCity];

            /* Sets the cost of the start node. */
            start.GCost = 0;

            /* Sets the heuristic cost. */
            start.HCost = Haversine.Distance(start, destination, DistanceType.km);

            /* Sets the f cost limit. */
            double fCostLimit = start.GCost + start.HCost;

            while (true)
            {
                /* Start IDA Star pathfinding search. */
                Node foundTemp =   await SearchPath(start, destination, start.GCost,fCostLimit);

                switch(foundTemp.Status)
                {
                    /* A new cost limit has been found. */
                    case SEARCHRETURN.BOUND:
                        fCostLimit = foundTemp.HCost;
                        break;
                    /* The path was found. */
                    case SEARCHRETURN.FOUND:
                        path = RetracePath(start, destination);
                        return path;
                       
                    /* No path has been found. */
                    case SEARCHRETURN.NOT_FOUND:
                        return null;
                }
            }


        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_currentNode"></param>
        /// <param name="_targetNode"></param>
        /// <param name="_gCost"></param>
        /// <param name="fCostLimit"></param>
        /// <returns></returns>
        public async Task<Node> SearchPath(Node currentNode, Node targetNode, double gCost, double fCostLimit)
        {
            Node retNode = new Node();
            Path<Node> path = new Path<Node>(currentNode);

            double distance(Node node1, Node node2) =>
                                                   node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                       etn => etn.Neighbor.Key == node2.Key).Cost;

            /* If the current node is the target, the search process will finish. */
            if (currentNode == targetNode)
            {
                retNode.Status = SEARCHRETURN.FOUND;
                return retNode;
            }

            double newFCostLimit = gCost + Haversine.Distance(currentNode, targetNode, DistanceType.km);

            if (newFCostLimit > fCostLimit)
            {
                retNode.Status = SEARCHRETURN.BOUND;
                retNode.HCost = newFCostLimit;
                return retNode;
            }

            if (!_visitedNodes.Contains(currentNode))
            {
                _visitedNodes.Add(currentNode);
            }

            foreach (Node vecin in path.LastStep.Neighbours)
            {
                if (_visitedNodes.Contains(vecin))
                {
                    continue;
                }

                if (!_visitedNodes.Contains(vecin))
                {
                    double costNouCatreVecin = distance(currentNode,vecin) + Haversine.Distance(currentNode, vecin, DistanceType.km);
                    if (costNouCatreVecin < vecin.GCost || !_visitedNodes.Contains(vecin))
                    {
                        vecin.GCost = costNouCatreVecin;
                        vecin.HCost = Haversine.Distance(vecin, targetNode, DistanceType.km);
                        vecin.PathParent = currentNode;
                    }

                    Node t = await SearchPath(vecin, targetNode, costNouCatreVecin, fCostLimit);
                    switch (t.Status)
                    {
                        case SEARCHRETURN.BOUND:
                            if (t.HCost < _costLimit)
                            {
                                _costLimit = t.HCost;
                            }
                            break;
                        case SEARCHRETURN.FOUND:
                            return t;
                        case SEARCHRETURN.NOT_FOUND:
                            continue;
                    }
                }

            }

            if (_costLimit == _minValue)
            {
                retNode.Status = SEARCHRETURN.NOT_FOUND;
            }
            else
            {
                retNode.HCost = _costLimit;
                retNode.Status = SEARCHRETURN.BOUND;
            }

            _visitedNodes.Remove(currentNode);
            return retNode;
        }

        /// <summary>
        /// Retrace the path to the target node.
        /// </summary>
        /// <param name="start">The city node from which the user will start.</param>
        /// <param name="destination">The city node which user will arrive.</param>
        /// <returns></returns>
        public List<string> RetracePath(Node start, Node destination)
        {
            List<string> path = new List<string>();
            Node currentNode = destination;

            path.Add(start.Key);
            while (currentNode != start)
            {
                path.Add(currentNode.Key);
                currentNode = currentNode.PathParent;
            }

            path.Reverse();

            return path;

        }
        #endregion




    }
}
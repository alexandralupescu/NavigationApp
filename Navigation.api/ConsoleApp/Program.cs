using Navigation.AStar.Implementations;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                // Creating the Graph...
                Graph graph = new Graph();

                //FillGraphWithGridMap(graph);

                DistanceType distanceType = DistanceType.km;

                FillGraphWithEarthMap(graph, distanceType);

                // Prints on the screen the distance from a city to its neighbors.
                // Used mainly for debug information.
                // DistanceBetweenNodes(graph, DistanceType.Kilometers);

                Console.WriteLine("These are the Cities you can choose as Start and Destination in Romania: \n");

                // Prints on screen the cities that you can choose as Start and Destination.
                foreach (Node n in graph.Nodes.Cast<Node>().OrderBy(n => n.Key))
                {
                    Console.WriteLine(n.Key);
                }

                string startCity = GetStartCity(graph);

                string destinationCity = GetDestinationCity(graph);

                Node start = graph.Nodes[startCity];

                Node destination = graph.Nodes[destinationCity];

                // Function which tells us the exact distance between two neighbours.
                Func<Node, Node, double> distance = (node1, node2) =>
                                                    node1.Neighbors.Cast<EdgeToNeighbor>().Single(
                                                        etn => etn.Neighbor.Key == node2.Key).Cost;

                // Estimation/Heuristic function (Manhattan distance)
                // It tells us the estimated distance between the last node on a proposed path and the destination node.
                //Func<Node, double> manhattanEstimation = n => Math.Abs(n.X - destination.X) + Math.Abs(n.Y - destination.Y);

                // Estimation/Heuristic function (Haversine distance)
                // It tells us the estimated distance between the last node on a proposed path and the destination node.
                Func<Node, double> haversineEstimation =
                    n => Haversine.Distance(n, destination, DistanceType.km);

                //Path<Node> shortestPath = FindPath(start, destination, distance, manhattanEstimation);
                Path<Node> shortestPath = FindPath(start, destination, distance, haversineEstimation);

                Console.WriteLine("\nThis is the shortest path based on the A* Search Algorithm:\n");

                // Prints the shortest path.
                foreach (Path<Node> path in shortestPath.Reverse())
                {
                    if (path.PreviousSteps != null)
                    {
                        Console.WriteLine(string.Format("From {0, -15}  to  {1, -15} -> Total cost = {2:#.###} {3}",
                                          path.PreviousSteps.LastStep.Key, path.LastStep.Key, path.TotalCost, distanceType));
                    }
                }

                Console.Write("\nDo you wanna try A* Search again? Yes or No? ");
            }
            while (Console.ReadLine().ToLower() == "yes");
        }
    }
    }
}

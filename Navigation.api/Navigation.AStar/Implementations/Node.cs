﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Navigation.AStar.Implementations
{

    /// <summary>
    /// A Node is uniquely identified by its string Key.  A Node also has a Data property of type object
    /// that can be used to store any extra information associated with the Node. 
    /// A Node has a Latitude and Longitude properites that represent the node coordinates on the Earth. 
    /// </summary>
    /// <remarks>
    /// The Node has a property of type AdjacencyList, which represents the node's neighbors.  To add a neighbor,
    /// the Node class exposes an AddDirected() method, which adds a directed edge with an (optional) weight to
    /// some other Node.  These methods are marked internal, and are called by the Graph class.
    /// </remarks>

    public partial class Node
    {

        #region Public Properties
        /// <summary>
        /// Returns the Node's Key.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Returns the Node's Data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Returns an AdjacencyList of the Node's neighbors.
        /// </summary>
        public AdjacencyList Neighbors { get; private set; }

        /// <summary>
        /// Returns the Node's Path Parent.
        /// </summary>
        public Node PathParent { get; set; }

        /// <summary>
        /// Returns the Node's Latitude location on Earth.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Returns the Node's Longitude location on Earth.
        /// </summary>
        public double Longitude { get; set; }
        #endregion

        #region Constructors
        public Node(string key, object data) : this(key, data, null)
        {
        }

        public Node(string key, object data, double latitude, double longitude)
            : this(key, data, latitude, longitude, null)
        {
        }

        public Node(string key, object data, AdjacencyList neighbors)
        {
            Key = key;
            Data = data;

            if (neighbors == null)
            {
                Neighbors = new AdjacencyList();
            }
            else
            {
                Neighbors = neighbors;
            }
        }
     
        public Node(string key, object data, double latitude, double longitude, AdjacencyList neighbors)
        {
            Key = key;
            Data = data;
            Latitude = latitude;
            Longitude = longitude;

            if (neighbors == null)
            {
                Neighbors = new AdjacencyList();
            }
            else
            {
                Neighbors = neighbors;
            }
        }
        #endregion

        #region Public Add Methods  
        /// <summary>
        /// Adds an unweighted, directed edge from this node to the passed-in Node n.
        /// </summary>
        internal void AddDirected(Node n)
        {
            AddDirected(new EdgeToNeighbor(n));
        }

        /// <summary>
        /// Adds a weighted, directed edge from this node to the passed-in Node n.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        internal void AddDirected(Node n, double cost)
        {
            AddDirected(new EdgeToNeighbor(n, cost));
        }

        /// <summary>
        /// Adds a weighted, directed edge from this node to the passed-in Node n.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        internal void AddDirected(Node n, int cost)
        {
            AddDirected(new EdgeToNeighbor(n, cost));
        }

        /// <summary>
        /// Adds an edge based on the data in the passed-in EdgeToNeighbor instance.
        /// </summary>
        internal void AddDirected(EdgeToNeighbor e)
        {
            Neighbors.Add(e);
        }
        #endregion
    }


}

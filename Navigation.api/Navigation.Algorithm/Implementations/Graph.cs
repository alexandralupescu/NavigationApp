﻿/**************************************************************************
 *                                                                        *
 *  File:        Graph.cs                                                 *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System;


namespace Navigation.Algorithm.Implementations
{
    /// <summary>
    /// The Graph class represents a graph, which is composed of a collection of nodes and edges. This Graph class
    /// maintains its collection of nodes using the NodeList class, which is a collection of TNode objects.
    /// It delegates the edge maintenance to the TNode class.  The TNode class maintains the edge information using
    /// the adjacency list technique.
    /// </summary>
    public class Graph
    {
        #region Private Members
        private NodeList _nodes;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor. Creates a new Graph class instance.
        /// </summary>
        public Graph()
        {
            _nodes = new NodeList();
        }

        /// <summary>
        /// Creates a new graph class instance based on a list of nodes.
        /// </summary>
        /// <param name="nodes">The list of nodes to populate the newly created Graph class with.</param>
        public Graph(NodeList nodes)
        {
            this._nodes = nodes;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears out all of the nodes in the graph.
        /// </summary>
        public virtual void Clear()
        {
            _nodes.Clear();
        }

        #region Adding TNode Methods
        /// <summary>
        /// Adds a new node to the graph.
        /// </summary>
        /// <param name="key">The key value of the node to add.</param>
        /// <param name="data">The data of the node to add.</param>
        /// <returns>A reference to the TNode that was created and added to the graph.</returns>
        /// <remarks>If there already exists a node in the graph with the same <b>key</b> value then an
        /// <b>ArgumentException</b> exception will be thrown.</remarks>
        public virtual Node AddNode(string key, object data)
        {
            /* Make sure the key is unique. */
            if (!_nodes.ContainsKey(key))
            {
                Node n = new Node(key, data);
                _nodes.Add(n);
                return n;
            }
            else
            {
                throw new ArgumentException("There already exists a node in the graph with key " + key);
            }
        }

        public virtual Node AddNode(string key, object data, double latitude, double longitude)
        {
            /* Make sure the key is unique. */
            if (!_nodes.ContainsKey(key))
            {
                Node n = new Node(key, data, latitude, longitude);

                _nodes.Add(n);

                return n;
            }
            else
            {
                throw new ArgumentException("There already exists a node in the graph with key " + key);
            }
        }

        /// <summary>
        /// Adds a new node to the graph.
        /// </summary>
        /// <param name="n">A node instance to add to the graph</param>
        /// <remarks>If there already exists a node in the graph with the same <b>key</b> value as <b>n</b> then an
        /// <b>ArgumentException</b> exception will be thrown.</remarks>
        public virtual void AddNode(Node n)
        {
            /* Make sure the key is unique. */
            if (!_nodes.ContainsKey(n.Key))
            {
                _nodes.Add(n);
            }

            else
            {
                throw new ArgumentException("There already exists a node in the graph with key " + n.Key);
            }

        }
        #endregion

        #region Adding Edge Methods
        /// <summary>
        /// Adds a directed edge from one node to another.
        /// </summary>
        /// <param name="uKey">The <b>Key</b> of the node from which the directed edge eminates.</param>
        /// <param name="vKey">The <b>Key</b> of the node from which the directed edge leads to.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(string uKey, string vKey)
        {
            /* Get references to uKey and vKey. */
            if (_nodes.ContainsKey(uKey) && (_nodes.ContainsKey(vKey)))
            {
                AddDirectedEdge(uKey, vKey, 0);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }
            
        }

        /// <summary>
        /// Adds a directed, weighted edge from one node to another.
        /// </summary>
        /// <param name="uKey">The <b>Key</b> of the node from which the directed edge eminates.</param>
        /// <param name="vKey">The <b>Key</b> of the node from which the directed edge leads to.</param>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(string uKey, string vKey, double cost)
        {
            /* Get references to uKey and vKey. */
            if (_nodes.ContainsKey(uKey) && _nodes.ContainsKey(vKey))
            {
                AddDirectedEdge(_nodes[uKey], _nodes[vKey], cost);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }

        }

        /// <summary>
        /// Adds a directed edge from one node to another.
        /// </summary>
        /// <param name="u">The node from which the directed edge eminates.</param>
        /// <param name="v">The node from which the directed edge leads to.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(Node u, Node v)
        {
            /* Make sure u and v are Nodes in this graph. */
            if (_nodes.ContainsKey(u.Key) && _nodes.ContainsKey(v.Key))
            {
                /* Add an edge from u -> v. */
                AddDirectedEdge(u, v, 0);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }
               
        }

        /// <summary>
        /// Adds a directed, weighted edge from one node to another.
        /// </summary>
        /// <param name="u">The node from which the directed edge eminates.</param>
        /// <param name="v">The node from which the directed edge leads to.</param>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddDirectedEdge(Node u, Node v, double cost)
        {
            /* Make sure u and v are Nodes in this graph. */
            if (_nodes.ContainsKey(u.Key) && _nodes.ContainsKey(v.Key))
            {
                /* Add an edge from u -> v. */
                u.AddDirected(v, cost);
            }

            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }

        }

        /// <summary>
        /// Adds an undirected edge from one node to another.
        /// </summary>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(string uKey, string vKey)
        {
            /* Get references to uKey and vKey. */
            if (_nodes.ContainsKey(uKey) && _nodes.ContainsKey(vKey))
            {
                AddUndirectedEdge(uKey, vKey, 0);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }               
        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If nodes with <b>uKey</b> and <b>vKey</b> do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(string uKey, string vKey, double cost)
        {
            /* Get references to uKey and vKey. */
            if (_nodes.ContainsKey(uKey) && _nodes.ContainsKey(vKey))
            {
                AddUndirectedEdge(_nodes[uKey], _nodes[vKey], cost);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }

        }

        /// <summary>
        /// Adds an undirected edge from one node to another.
        /// </summary>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(Node u, Node v)
        {
            /* Make sure u and v are Nodes in this graph. */
            if (_nodes.ContainsKey(u.Key) && _nodes.ContainsKey(v.Key))
            {
                /* Add an edge from u -> v. */
                AddUndirectedEdge(u, v, 0);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }
        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(Node u, Node v, int cost)
        {
            /* Make sure u and v are Nodes in this graph. */
            if (_nodes.ContainsKey(u.Key) && _nodes.ContainsKey(v.Key))
            {
                /* Add an edge from u -> v and from v -> u. */
                u.AddDirected(v, cost);
                v.AddDirected(u, cost);
            }
            else
            {
                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");
            }

        }

        /// <summary>
        /// Adds an undirected, weighted edge from one node to another.
        /// </summary>
        /// <param name="cost">The weight of the edge.</param>
        /// <remarks>If the passed-in nodes do not exist in the graph, an <b>ArgumentException</b>
        /// exception is thrown.</remarks>
        public virtual void AddUndirectedEdge(Node u, Node v, double cost)
        {
            /* Make sure u and v are Nodes in this graph. */
            if (_nodes.ContainsKey(u.Key) && _nodes.ContainsKey(v.Key))
            {
                /* Add an edge from u -> v and from v -> u. */
                u.AddDirected(v, cost);
                v.AddDirected(u, cost);
            }
            else
            {

                /* One or both of the nodes were not found in the graph. */
                throw new ArgumentException("One or both of the nodes supplied were not members of the graph.");

            }
        }

        #endregion

        #region Contains Methods
        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="n">The node to check for in the graph.</param>
        /// <returns><b>True</b> if the node <b>n</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool Contains(Node n)
        {
            return Contains(n.Key);
        }

        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="key">The key of the node to check for in the graph.</param>
        /// <returns><b>True</b> if a node with key <b>key</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool Contains(string key)
        {
            return _nodes.ContainsKey(key);
        }
        #endregion
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the number of nodes in the graph.
        /// </summary>
        public virtual double Count
        {
            get
            {
                return _nodes.Count;
            }
        }

        /// <summary>
        /// Returns a reference to the set of nodes in the graph.
        /// </summary>
        public virtual NodeList Nodes
        {
            get
            {
                return _nodes;
            }
        }
        #endregion
    }
}

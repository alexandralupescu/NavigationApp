/**************************************************************************
 *                                                                        *
 *  File:        NodeList.cs                                              *
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
using System;
using System.Collections;

namespace Navigation.AStar.Implementations
{
    /// <summary>
	/// The NodeList class represents a collection of nodes.  Internally, it uses a Hashtable instance to provide
	/// fast lookup via a <see cref="Node"/> class's <b>Key</b> value.  The <see cref="Graph"/> class maintains its
	/// list of nodes via this class.
	/// </summary>
	public class NodeList : IEnumerable
    {
        #region Private Members
        private Hashtable _data = new Hashtable();
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a new Node to the NodeList.
        /// </summary>
        public virtual void Add(Node n)
        {
            _data.Add(n.Key, n);
        }

        /// <summary>
        /// Removes a Node from the NodeList.
        /// </summary>
        public virtual void Remove(Node n)
        {
            _data.Remove(n.Key);
        }

        /// <summary>
        /// Determines if a node with a specified <b>Key</b> value exists in the NodeList.
        /// </summary>
        /// <param name="key">The <b>Key</b> value to search for.</param>
        /// <returns><b>True</b> if a Node with the specified <b>Key</b> exists in the NodeList; <b>False</b> otherwise.</returns>
        public virtual bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        /// <summary>
        /// Clears out all of the nodes from the NodeList.
        /// </summary>
        public virtual void Clear()
        {
            _data.Clear();
        }
        #endregion

        #region IEnumerator Members
        /// <summary>
        /// Returns an enumerator that can be used to iterate through the Nodes.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return new NodeListEnumerator(_data.GetEnumerator());
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns a particular <see cref="Node"/> instance by index.
        /// </summary>
        public virtual Node this[string key]
        {
            get
            {
                return (Node)_data[key];
            }
        }

        /// <summary>
        /// Returns the number of nodes in the NodeList.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return _data.Count;
            }
        }
        #endregion

        #region NodeList Enumerator
        /// <summary>
        /// The NodeListEnumerator method is a custom enumerator for the NodeList object.  It essentially serves
        /// as an enumerator over the NodeList's Hashtable class, but rather than returning DictionaryEntry values,
        /// it returns just the Node object.
        /// This allows  using the Graph class to use a foreach to enumerate the Nodes in the graph.
        /// </summary>
        public class NodeListEnumerator : IEnumerator, IDisposable
        {
            /// <summary>
            /// Enumerates the elements of a nongeneric dictionary.
            /// </summary>
            IDictionaryEnumerator list;

            /// <summary>
            /// Construct the IDictionaryEnumerator.
            /// </summary>
            /// <param name="coll"></param>
            public NodeListEnumerator(IDictionaryEnumerator coll)
            {
                list = coll;
            }

            /// <summary>
            /// Restart the enumeration from the initial position.
            /// </summary>
            public void Reset()
            {
                list.Reset();
            }

            /// <summary>
            /// Advance to the next item.
            /// </summary>
            public bool MoveNext()
            {
                return list.MoveNext();
            }

            /// <summary>
            /// Return the current item.
            /// </summary>
            public Node Current
            {
                get
                {
                    return (Node)((DictionaryEntry)list.Current).Value;
                }
            }

            /// <summary>
            /// The current property on the IEnumerator interface.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return (Current);
                }
            }

            /// <summary>
            /// Dispose of unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                list = null;
            }
        }
        #endregion
    }
}

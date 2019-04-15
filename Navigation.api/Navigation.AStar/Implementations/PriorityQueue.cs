/**************************************************************************
 *                                                                        *
 *  File:        PriorityQueue.cs                                         *
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Navigation.AStar.Implementations
{
    /// <summary>
    /// Immutable priority queue.
    /// </summary>
    /// <remarks>
    /// In order to make the A* algorithm work, we need to get the lowest-estimated cost path.
    /// The standard data structure for doing is called a priority queue. Priority queues are so-called because 
    /// they are tipically used to store a list where each element has an associated priority.
    /// The "highest priority" path is the one with the least estimated cost.
    /// <typeparam name="P"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class PriorityQueue<P, V> : IEnumerable
    {
        #region Private Members
        /// <summary>
        /// Sorted dictionary list.
        /// </summary>
        private SortedDictionary<P, Queue<V>> _list = new SortedDictionary<P, Queue<V>>();
        #endregion

        #region Queue Operations
        /// <summary>
        /// Add an element to the tail of a queue.
        /// </summary>
        /// <param name="priority">Priority of an element.</param>
        /// <param name="value">Value of the element.</param>
        public void Enqueue(P priority, V value)
        {

            if (!_list.TryGetValue(priority, out Queue<V> q))
            {
                q = new Queue<V>();
                _list.Add(priority, q);
            }

            q.Enqueue(value);
        }

        /// <summary>
        /// Remove the first element from the head of a queue.
        /// </summary>
        public V Dequeue()
        {

            /* Will throw if there isn't any first element. */
            var pair = _list.First();
            var v = pair.Value.Dequeue();

            /* Nothing left of the top priority. */
            if (pair.Value.Count == 0)
            {
                _list.Remove(pair.Key);
            }
               
            return v;
        }

        /// <summary>
        /// Verify if the queue is empty (has elements).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return !_list.Any();
            }
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that iterates through a list.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        #endregion
    }
}

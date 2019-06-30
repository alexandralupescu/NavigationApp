/**************************************************************************
 *                                                                        *
 *  File:        AdjacencyList.cs                                         *
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

namespace Navigation.Algorithm.Implementations
{

    /// <summary>
    /// AdjacencyList maintains a list of neighbors for a particular <see cref="Node"/>.  It is derived from CollectionBase
    /// and provides a strongly-typed collection of <see cref="EdgeToNeighbor"/> instances.
    /// </summary>
    public class AdjacencyList : CollectionBase
    {
        /// <summary>
        /// Adds a new <see cref="EdgeToNeighbor"/> instance to the AdjacencyList.
        /// </summary>
        /// <param name="e">The <see cref="EdgeToNeighbor"/> instance to add.</param>
        protected internal virtual void Add(EdgeToNeighbor e)
        {
            InnerList.Add(e);
        }

        /// <summary>
        /// Returns a particular <see cref="EdgeToNeighbor"/> instance by index.
        /// </summary>
        public virtual EdgeToNeighbor this[int index]
        {
            get { return (EdgeToNeighbor)base.InnerList[index]; }
            set { InnerList[index] = value; }
        }
    }
}

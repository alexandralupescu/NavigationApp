/**************************************************************************
 *                                                                        *
 *  File:       EdgeToNeighbor.cs                                         *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/

namespace Navigation.Algorithm.Implementations
{
    /// <summary>
    /// EdgeToNeighbor represents an edge eminating from one <see cref="Node"/> to its neighbor. The EdgeToNeighbor
    /// class, then, contains a reference to the neighbor and the weight of the edge.
    /// </summary>
    public class EdgeToNeighbor
    {
        #region Public Properties
        /// <summary>
        /// The weight of the edge.
        /// </summary>
        /// <remarks>A value of 0 would indicate that there is no weight, and is the value used when an unweighted
        /// edge is added via the <see cref="Graph"/> class.</remarks>
        public virtual double Cost { get; private set; }

        /// <summary>
        /// The neighbor the edge is leading to.
        /// </summary>
        public virtual Node Neighbor { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor. Creates a new EdgeToNeighbor class instance.
        /// </summary>
        /// <param name="neighbor"></param>
        public EdgeToNeighbor(Node neighbor) : this(neighbor, 0)
        {
        }

        public EdgeToNeighbor(Node neighbor, double cost)
        {
            Cost = cost;
            Neighbor = neighbor;
        }
        #endregion


    }
}

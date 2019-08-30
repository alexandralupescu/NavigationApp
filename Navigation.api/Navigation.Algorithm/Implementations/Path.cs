/**************************************************************************
 *                                                                        *
 *  File:        Path.cs                                                  *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System.Collections;
using System.Collections.Generic;

namespace Navigation.Algorithm.Implementations
{
    /// <summary>
    /// Immutable stack of nodes which tracks the total cost of the whole path.
    /// </summary>
    /// <typeparam name="Node">Current node of the path.</typeparam>
    public class Path<Node> : IEnumerable<Path<Node>>
    {

        #region Public Properties
        /// <summary>
        /// Keep the last step in the list.
        /// </summary>
        public Node LastStep { get; private set; }

        /// <summary>
        /// Keep the previous steps in the list.
        /// </summary>
        public Path<Node> PreviousSteps { get; private set; }

        /// <summary>
        /// Total cost of the path.
        /// </summary>
        public double TotalCost { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Path constructor. Initialize step.
        /// </summary>
        /// <param name="lastStep">Last step in the list (last element - city reached).</param>
        /// <param name="previousSteps">Previous steps in the list (in our case the cities reached).</param>
        /// <param name="totalCost">Total cost of the path.</param>
        private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
        {
            LastStep = lastStep;

            PreviousSteps = previousSteps;

            TotalCost = totalCost;
        }

        public Path(Node start) : this(start, null, 0) { }
        #endregion

        #region Public Methods
        /// <summary>
        /// Method that will add in the list one node.
        /// </summary>
        /// <param name="step">The Node object that will contain all the additional information.</param>
        /// <param name="stepCost">The cost of the node.</param>
        public Path<Node> AddStep(Node step, double stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Returns an enumerator that iterates through a list.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Path<Node>> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
            {
                yield return p;
            }

        }

        /// <summary>
        /// Returns an enumerator that iterates through a list.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

    }
}

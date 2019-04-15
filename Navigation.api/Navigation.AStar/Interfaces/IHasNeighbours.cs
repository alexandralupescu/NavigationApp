/**************************************************************************
 *                                                                        *
 *  File:        IHasNeighbours.cs                                        *
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
using System.Collections.Generic;

namespace Navigation.AStar.Implementations
{
    /// <summary>
    /// Interface used to store neighbours of a Node.
    /// </summary>
    /// <typeparam name="N"><b>N</b> Represents one Node and it will return the neighbours list of the current Node.</typeparam>
    public interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
    }
}

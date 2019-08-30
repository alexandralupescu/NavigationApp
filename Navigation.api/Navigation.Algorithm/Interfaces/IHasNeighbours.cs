/**************************************************************************
 *                                                                        *
 *  File:        IHasNeighbours.cs                                        *
 *  Copyright:   (c) 2019, Maria-Alexandra Lupescu                        *
 *  E-mail:      mariaalexandra.lupescu@yahoo.com                         *             
 *  Description: Apply heuristic search algorithms in travel planning     *
 **************************************************************************/
using System.Collections.Generic;

namespace Navigation.Algorithm.Interfaces
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

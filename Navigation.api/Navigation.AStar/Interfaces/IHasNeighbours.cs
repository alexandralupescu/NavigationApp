using System;
using System.Collections.Generic;
using System.Text;

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

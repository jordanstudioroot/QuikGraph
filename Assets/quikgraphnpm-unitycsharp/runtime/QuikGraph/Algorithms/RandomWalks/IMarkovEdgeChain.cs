using System;
using QuikGraph.Utils;


namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// A Markov edges chain.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IMarkovEdgeChain<TVertex, TEdge> : IEdgeChain<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Random number generator for a Markov process to do random walks.
        /// </summary>
        
// TODO: Add options for using unity native random values or
//       proprietary random number generator.
        CryptoRandom Rand { get; set; }
    }
}

using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// An implicit graph with vertices of type <typeparamref name="TVertex"/>
    /// and edges of type <typeparamref name="TEdge"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IImplicitGraph<TVertex, TEdge> : IGraph<TVertex, TEdge>, IImplicitVertexSet<TVertex>
         where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Determines whether there are out-edges associated to <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>True if <paramref name="vertex"/> has no out-edges, false otherwise.</returns>
        [JBPure]
        bool IsOutEdgesEmpty([JBNotNull] TVertex vertex);

        /// <summary>
        /// Gets the count of out-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>The count of out-edges of <paramref name="vertex"/>.</returns>
        [JBPure]
        int OutDegree([JBNotNull] TVertex vertex);

        /// <summary>
        /// Gets the out-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>An enumeration of the out-edges of <paramref name="vertex"/>.</returns>
        [JBPure]
        [JBNotNull, ItemNotNull]
        IEnumerable<TEdge> OutEdges([JBNotNull] TVertex vertex);

        /// <summary>
        /// Tries to get the out-edges of <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="edges">Out-edges.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and out-edges were found, false otherwise.</returns>
        [JBPure]
        [JBContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetOutEdges([JBNotNull] TVertex vertex, [ItemNotNull] out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the out-edge of <paramref name="vertex"/> at position <paramref name="index"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="index">The index.</param>
        /// <returns>The out-edge at position <paramref name="index"/>.</returns>
        [JBPure]
        [JBNotNull]
        TEdge OutEdge([JBNotNull] TVertex vertex, int index);
    }
}

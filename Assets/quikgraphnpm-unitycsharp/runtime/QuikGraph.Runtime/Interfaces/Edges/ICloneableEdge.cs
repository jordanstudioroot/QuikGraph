


namespace QuikGraph
{
    /// <summary>
    /// Represents a cloneable edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface ICloneableEdge<TVertex, out TEdge> : IEdge<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Clones this edge content to a different pair of <paramref name="source"/>
        /// and <paramref name="target"/> vertices.
        /// </summary>
        /// <param name="source">The source vertex of the new edge.</param>
        /// <param name="target">The target vertex of the new edge.</param>
        /// <returns>A clone of the edge with new source and target vertices.</returns>
        
        
        TEdge Clone( TVertex source,  TVertex target);
    }
}

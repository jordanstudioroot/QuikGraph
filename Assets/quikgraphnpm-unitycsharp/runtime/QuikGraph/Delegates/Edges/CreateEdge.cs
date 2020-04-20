#if SUPPORTS_SERIALIZATION
using System;
#endif
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create an edge in a graph between two vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="graph">Graph in with adding the edge.</param>
    /// <param name="source">Edge source vertex.</param>
    /// <param name="target">Edge target vertex.</param>
    /// <returns>The created edge.</returns>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    [JBNotNull]
    public delegate TEdge CreateEdge<TVertex, TEdge>(
        [JBNotNull] IVertexListGraph<TVertex, TEdge> graph,
        [JBNotNull] TVertex source,
        [JBNotNull] TVertex target)
        where TEdge : IEdge<TVertex>;
}

using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to perform an action involving the 2 edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="edge">First edge.</param>
    /// <param name="targetEdge">Second edge.</param>
    public delegate void EdgeEdgeAction<TVertex, in TEdge>([JBNotNull] TEdge edge, [JBNotNull] TEdge targetEdge)
        where TEdge : IEdge<TVertex>;
}

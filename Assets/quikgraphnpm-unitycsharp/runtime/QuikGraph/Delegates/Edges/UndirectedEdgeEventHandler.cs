using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate for an handler dealing with an undirected edge.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void UndirectedEdgeAction<TVertex, TEdge>([JBNotNull] object sender, [JBNotNull] UndirectedEdgeEventArgs<TVertex, TEdge> args)
        where TEdge : IEdge<TVertex>;
}

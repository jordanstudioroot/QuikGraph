using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to for an handler dealing with a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void VertexEventHandler<TVertex>([JBNotNull] object sender, [JBNotNull] VertexEventArgs<TVertex> args);
}

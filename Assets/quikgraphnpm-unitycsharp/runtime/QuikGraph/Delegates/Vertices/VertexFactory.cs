using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create a vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <returns>The created vertex.</returns>
    [JBNotNull]
    public delegate TVertex VertexFactory<out TVertex>();
}

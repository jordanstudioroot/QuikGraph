using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to create an identifiable vertex.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="id">Vertex id.</param>
    /// <returns>The created vertex.</returns>
    [JBNotNull]
    public delegate TVertex IdentifiableVertexFactory<out TVertex>([JBNotNull] string id);
}

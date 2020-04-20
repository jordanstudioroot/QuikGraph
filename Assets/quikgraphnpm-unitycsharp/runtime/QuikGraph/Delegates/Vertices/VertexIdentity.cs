using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Delegate to compute the identity of the given <paramref name="vertex"/>.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <param name="vertex">Vertex to compute identity.</param>
    /// <returns>The <paramref name="vertex"/> identity.</returns>
    [JBNotNull]
    public delegate string VertexIdentity<in TVertex>([JBNotNull] TVertex vertex);
}

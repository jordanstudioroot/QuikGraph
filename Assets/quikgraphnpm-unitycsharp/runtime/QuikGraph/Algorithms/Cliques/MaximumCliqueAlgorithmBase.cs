using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;

namespace QuikGraph.Algorithms.Cliques
{
    /// <summary>
    /// Base class for all maximum clique graph algorithm.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public abstract class MaximumCliqueAlgorithmBase<TVertex, TEdge> : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected MaximumCliqueAlgorithmBase(
            [JBCanBeNull] IAlgorithmComponent host,
            [JBNotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(host, visitedGraph)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumCliqueAlgorithmBase{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        protected MaximumCliqueAlgorithmBase([JBNotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph)
            : base(visitedGraph)
        {
        }
    }
}

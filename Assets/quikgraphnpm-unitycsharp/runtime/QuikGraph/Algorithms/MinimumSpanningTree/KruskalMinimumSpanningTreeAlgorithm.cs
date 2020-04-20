using System;
using System.Diagnostics;
using JetBrains.Annotations;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;

namespace QuikGraph.Algorithms.MinimumSpanningTree
{
    /// <summary>
    /// Kruskal minimum spanning tree algorithm implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class KruskalMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        : AlgorithmBase<IUndirectedGraph<TVertex, TEdge>>
        , IMinimumSpanningTreeAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        [JBNotNull]
        private readonly Func<TEdge, double> _edgeWeights;

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public KruskalMinimumSpanningTreeAlgorithm(
            [JBNotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [JBNotNull] Func<TEdge, double> edgeWeights)
            : this(null, visitedGraph, edgeWeights)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KruskalMinimumSpanningTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public KruskalMinimumSpanningTreeAlgorithm(
            [JBCanBeNull] IAlgorithmComponent host,
            [JBNotNull] IUndirectedGraph<TVertex, TEdge> visitedGraph,
            [JBNotNull] Func<TEdge, double> edgeWeights)
            : base(host, visitedGraph)
        {
            _edgeWeights = edgeWeights ?? throw new ArgumentNullException(nameof(edgeWeights));
        }

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        private void OnExamineEdge([JBNotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            ExamineEdge?.Invoke(edge);
        }

        #region ITreeBuilderAlgorithm<TVertex,TEdge>

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge([JBNotNull] TEdge edge)
        {
            Debug.Assert(edge != null);

            TreeEdge?.Invoke(edge);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            ICancelManager cancelManager = Services.CancelManager;

            var sets = new ForestDisjointSet<TVertex>(VisitedGraph.VertexCount);
            foreach (TVertex vertex in VisitedGraph.Vertices)
                sets.MakeSet(vertex);

            if (cancelManager.IsCancelling)
                return;

            var queue = new BinaryQueue<TEdge, double>(_edgeWeights);
            foreach (TEdge edge in VisitedGraph.Edges)
                queue.Enqueue(edge);

            if (cancelManager.IsCancelling)
                return;

            while (queue.Count > 0)
            {
                TEdge edge = queue.Dequeue();
                OnExamineEdge(edge);

                if (!sets.AreInSameSet(edge.Source, edge.Target))
                {
                    OnTreeEdge(edge);
                    sets.Union(edge.Source, edge.Target);
                }
            }
        }

        #endregion
    }
}

using System;
using System.Diagnostics;
using QuikGraph.Algorithms.Search;
using QuikGraph.Algorithms.Services;
using QuikGraph.Collections;


namespace QuikGraph.Algorithms.ShortestPath
{
    /// <summary>
    /// A single source shortest path algorithm for undirected graph
    /// with positive distances.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [Serializable]
    public sealed class UndirectedDijkstraShortestPathAlgorithm<TVertex, TEdge>
        : UndirectedShortestPathAlgorithmBase<TVertex, TEdge>
        , IUndirectedVertexPredecessorRecorderAlgorithm<TVertex, TEdge>
        , IDistanceRecorderAlgorithm<TVertex>
        where TEdge : IEdge<TVertex>
    {
        private IPriorityQueue<TVertex> _vertexQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDijkstraShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        public UndirectedDijkstraShortestPathAlgorithm(
             IUndirectedGraph<TVertex, TEdge> visitedGraph,
             Func<TEdge, double> edgeWeights)
            : base(null, visitedGraph, edgeWeights)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDijkstraShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public UndirectedDijkstraShortestPathAlgorithm(
             IUndirectedGraph<TVertex, TEdge> visitedGraph,
             Func<TEdge, double> edgeWeights,
             IDistanceRelaxer distanceRelaxer)
            : this(null, visitedGraph, edgeWeights, distanceRelaxer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndirectedDijkstraShortestPathAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeWeights">Function that computes the weight for a given edge.</param>
        /// <param name="distanceRelaxer">Distance relaxer.</param>
        public UndirectedDijkstraShortestPathAlgorithm(
             IAlgorithmComponent host,
             IUndirectedGraph<TVertex, TEdge> visitedGraph,
             Func<TEdge, double> edgeWeights,
             IDistanceRelaxer distanceRelaxer)
            : base(host, visitedGraph, edgeWeights, distanceRelaxer)
        {
        }

        [Conditional("DEBUG")]
        private void AssertHeap()
        {
            if (_vertexQueue.Count == 0)
                return;

            TVertex top = _vertexQueue.Peek();
            TVertex[] vertices = _vertexQueue.ToArray();
            for (int i = 1; i < vertices.Length; ++i)
            {
                if (Distances[top] > Distances[vertices[i]])
                    Debug.Assert(false);
            }
        }

        #region Events

        /// <inheritdoc />
        public event VertexAction<TVertex> InitializeVertex;

        /// <inheritdoc />
        public event VertexAction<TVertex> StartVertex;

        /// <inheritdoc />
        public event VertexAction<TVertex> DiscoverVertex;

        /// <summary>
        /// Fired when a vertex is going to be analyzed.
        /// </summary>
        public event VertexAction<TVertex> ExamineVertex;

        /// <summary>
        /// Fired when an edge is going to be analyzed.
        /// </summary>
        public event EdgeAction<TVertex, TEdge> ExamineEdge;

        /// <inheritdoc />
        public event VertexAction<TVertex> FinishVertex;

        /// <summary>
        /// Fired when relax of an edge does not decrease distance.
        /// </summary>
        public event UndirectedEdgeAction<TVertex, TEdge> EdgeNotRelaxed;

        private void OnEdgeNotRelaxed( TEdge edge, bool reversed)
        {
            Debug.Assert(edge != null);

            EdgeNotRelaxed?.Invoke(this, new UndirectedEdgeEventArgs<TVertex, TEdge>(edge, reversed));
        }

        private void OnDijkstraTreeEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            Debug.Assert(args != null);

            bool decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
                OnTreeEdge(args.Edge, args.Reversed);
            else
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
        }

        private void OnGrayTarget(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> args)
        {
            Debug.Assert(args != null);

            bool decreased = Relax(args.Edge, args.Source, args.Target);
            if (decreased)
            {
                _vertexQueue.Update(args.Target);
                AssertHeap();
                OnTreeEdge(args.Edge, args.Reversed);
            }
            else
            {
                OnEdgeNotRelaxed(args.Edge, args.Reversed);
            }
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            double initialDistance = DistanceRelaxer.InitialDistance;
            // Initialize colors and distances
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                VerticesColors.Add(vertex, GraphColor.White);
                Distances.Add(vertex, initialDistance);
            }

            _vertexQueue = new FibonacciQueue<TVertex, double>(DistancesIndexGetter());
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            if (TryGetRootVertex(out TVertex root))
            {
                AssertRootInGraph(root);
                ComputeFromRoot(root);
            }
            else
            {
                foreach (TVertex vertex in VisitedGraph.Vertices)
                {
                    if (VerticesColors[vertex] == GraphColor.White)
                        ComputeFromRoot(vertex);
                }
            }
        }

        #endregion

        private void ComputeFromRoot( TVertex rootVertex)
        {
            Debug.Assert(rootVertex != null);
            Debug.Assert(VisitedGraph.ContainsVertex(rootVertex));
            Debug.Assert(VerticesColors[rootVertex] == GraphColor.White);

            VerticesColors[rootVertex] = GraphColor.Gray;
            Distances[rootVertex] = 0;
            ComputeNoInit(rootVertex);
        }

        private void ComputeNoInit( TVertex root)
        {
            Debug.Assert(root != null);
            Debug.Assert(VisitedGraph.ContainsVertex(root));

            UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge> bfs = null;
            try
            {
                bfs = new UndirectedBreadthFirstSearchAlgorithm<TVertex, TEdge>(
                    this,
                    VisitedGraph,
                    _vertexQueue,
                    VerticesColors);

                bfs.InitializeVertex += InitializeVertex;
                bfs.DiscoverVertex += DiscoverVertex;
                bfs.StartVertex += StartVertex;
                bfs.ExamineEdge += ExamineEdge;
#if DEBUG
                bfs.ExamineEdge += edge => AssertHeap();
#endif
                bfs.ExamineVertex += ExamineVertex;
                bfs.FinishVertex += FinishVertex;

                bfs.TreeEdge += OnDijkstraTreeEdge;
                bfs.GrayTarget += OnGrayTarget;

                bfs.Visit(root);
            }
            finally
            {
                if (bfs != null)
                {
                    bfs.InitializeVertex -= InitializeVertex;
                    bfs.DiscoverVertex -= DiscoverVertex;
                    bfs.StartVertex -= StartVertex;
                    bfs.ExamineEdge -= ExamineEdge;
                    bfs.ExamineVertex -= ExamineVertex;
                    bfs.FinishVertex -= FinishVertex;

                    bfs.TreeEdge -= OnDijkstraTreeEdge;
                    bfs.GrayTarget -= OnGrayTarget;
                }
            }
        }
    }
}

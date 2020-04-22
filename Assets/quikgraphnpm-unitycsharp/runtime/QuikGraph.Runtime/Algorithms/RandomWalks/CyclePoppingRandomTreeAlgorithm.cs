using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using QuikGraph.Algorithms.Services;
using QuikGraph.Utils;


namespace QuikGraph.Algorithms.RandomWalks
{
    /// <summary>
    /// Wilson-Propp Cycle-Popping algorithm for Random Tree Generation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [Serializable]
    public sealed class CyclePoppingRandomTreeAlgorithm<TVertex, TEdge>
        : RootedAlgorithmBase<TVertex, IVertexListGraph<TVertex, TEdge>>
        , IVertexColorizerAlgorithm<TVertex>
        , ITreeBuilderAlgorithm<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        public CyclePoppingRandomTreeAlgorithm( IVertexListGraph<TVertex, TEdge> visitedGraph)
            : this(visitedGraph, new NormalizedMarkovEdgeChain<TVertex, TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        public CyclePoppingRandomTreeAlgorithm(
             IVertexListGraph<TVertex, TEdge> visitedGraph,
             IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : this(null, visitedGraph, edgeChain)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CyclePoppingRandomTreeAlgorithm{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="host">Host to use if set, otherwise use this reference.</param>
        /// <param name="visitedGraph">Graph to visit.</param>
        /// <param name="edgeChain">Edge chain strategy to use.</param>
        public CyclePoppingRandomTreeAlgorithm(
             IAlgorithmComponent host,
             IVertexListGraph<TVertex, TEdge> visitedGraph,
             IMarkovEdgeChain<TVertex, TEdge> edgeChain)
            : base(host, visitedGraph)
        {
            EdgeChain = edgeChain ?? throw new ArgumentNullException(nameof(edgeChain));
        }

        /// <summary>
        /// Stores vertices associated to their colors (treatment state).
        /// </summary>
        
        public IDictionary<TVertex, GraphColor> VerticesColors { get; } = new Dictionary<TVertex, GraphColor>();

        #region IVertexColorizerAlgorithm<TVertex>

        /// <inheritdoc />
        public GraphColor GetVertexColor(TVertex vertex)
        {
            if (VerticesColors.TryGetValue(vertex, out GraphColor color))
                return color;
            throw new VertexNotFoundException();
        }

        #endregion

        /// <summary>
        /// Edge chain strategy for the random walk.
        /// </summary>
        
        public IMarkovEdgeChain<TVertex, TEdge> EdgeChain { get; }

        
        private Random _rand =
            new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Gets or sets the random number generator used in <see cref="RandomTree"/>.
        /// </summary>
        
        public Random Rand
        {
            get => _rand;
            set => _rand = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Map vertices associated to their edge successors.
        /// </summary>
        
        public IDictionary<TVertex, TEdge> Successors { get; } = new Dictionary<TVertex, TEdge>();

        #region Events

        /// <summary>
        /// Fired when a vertex is initialized.
        /// </summary>
        public event VertexAction<TVertex> InitializeVertex;

        private void OnInitializeVertex( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            InitializeVertex?.Invoke(vertex);
        }

        /// <summary>
        /// Fired when a vertex is treated and considered as in the random tree.
        /// </summary>
        public event VertexAction<TVertex> FinishVertex;

        private void OnFinishVertex( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            FinishVertex?.Invoke(vertex);
        }

        /// <inheritdoc />
        public event EdgeAction<TVertex, TEdge> TreeEdge;

        private void OnTreeEdge( TEdge edge)
        {
            Debug.Assert(edge != null);

            TreeEdge?.Invoke(edge);
        }

        /// <summary>
        /// Fired when a vertex is removed from the random tree.
        /// </summary>
        public event VertexAction<TVertex> ClearTreeVertex;

        private void OnClearTreeVertex( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            ClearTreeVertex?.Invoke(vertex);
        }

        #endregion

        #region AlgorithmBase<TGraph>

        /// <inheritdoc />
        protected override void Initialize()
        {
            base.Initialize();

            Successors.Clear();
            VerticesColors.Clear();
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                VerticesColors.Add(vertex, GraphColor.White);
                OnInitializeVertex(vertex);
            }
        }

        /// <inheritdoc />
        protected override void InternalCompute()
        {
            TVertex root = GetAndAssertRootInGraph();

            // Process root
            ClearTree(root);
            SetInTree(root);

            ICancelManager cancelManager = Services.CancelManager;
            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    break;

                // First pass: exploration
                ExplorationPass(vertex);

                // Second pass: coloration
                ColorizationPass(vertex);
            }
        }

        private void ExplorationPass( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            var visitedEdges = new Dictionary<TEdge, int>();
            TVertex current = vertex;
            while (NotInTree(current) && TryGetSuccessor(visitedEdges, current, out TEdge successor))
            {
                visitedEdges[successor] = 0;
                Tree(current, successor);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }

        private void ColorizationPass( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            TVertex current = vertex;
            while (NotInTree(current))
            {
                SetInTree(current);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }

        #endregion

        private bool NotInTree( TVertex vertex)
        {
            return VerticesColors[vertex] == GraphColor.White;
        }

        private void SetInTree( TVertex vertex)
        {
            VerticesColors[vertex] = GraphColor.Black;
            OnFinishVertex(vertex);
        }

        private bool TryGetSuccessor( IDictionary<TEdge, int> visited,  TVertex vertex, out TEdge successor)
        {
            IEnumerable<TEdge> outEdges = VisitedGraph.OutEdges(vertex);
            IEnumerable<TEdge> edges = outEdges.Where(edge => !visited.ContainsKey(edge));
            return EdgeChain.TryGetSuccessor(edges, vertex, out successor);
        }

        private void Tree( TVertex vertex,  TEdge next)
        {
            Debug.Assert(vertex != null);
            Debug.Assert(next != null);

            Successors[vertex] = next;
            OnTreeEdge(next);
        }

        private bool TryGetNextInTree( TVertex vertex, out TVertex next)
        {
            if (Successors.TryGetValue(vertex, out TEdge nextEdge))
            {
                next = nextEdge.Target;
                return true;
            }

            next = default(TVertex);
            return false;
        }

        private bool Chance(double eps)
        {
            return Rand.NextDouble() <= eps;
        }

        private void ClearTree( TVertex vertex)
        {
            Successors[vertex] = default(TEdge);
            OnClearTreeVertex(vertex);
        }

        /// <summary>
        /// Runs a random tree generation starting at <paramref name="root"/> vertex.
        /// </summary>
        /// <param name="root">Tree starting vertex.</param>
        public void RandomTreeWithRoot( TVertex root)
        {
            if (!VisitedGraph.ContainsVertex(root))
                throw new ArgumentException("The vertex must be in the graph.", nameof(root));

            SetRootVertex(root);
            Compute();
        }

        /// <summary>
        /// Runs a random tree generation.
        /// </summary>
        public void RandomTree()
        {
            ICancelManager cancelManager = Services.CancelManager;

            double epsilon = 1;
            bool success;
            do
            {
                if (cancelManager.IsCancelling)
                    break;

                epsilon /= 2;
                success = Attempt(epsilon);
            } while (!success);
        }

        private bool Attempt(double epsilon)
        {
            Initialize();
            int numRoots = 0;
            ICancelManager cancelManager = Services.CancelManager;

            foreach (TVertex vertex in VisitedGraph.Vertices)
            {
                if (cancelManager.IsCancelling)
                    break;

                // First pass: exploration
                if (!Explore(epsilon, vertex, ref numRoots))
                    return false;

                // Second pass: coloration
                Colorize(vertex);
            }

            return true;
        }

        private bool Explore(double eps,  TVertex vertex, ref int numRoots)
        {
            var visited = new Dictionary<TEdge, int>();
            TVertex current = vertex;
            while (NotInTree(current))
            {
                if (Chance(eps))
                {
                    ClearTree(current);
                    SetInTree(current);
                    ++numRoots;
                    if (numRoots > 1)
                        return false;
                }
                else
                {
                    if (!TryGetSuccessor(visited, current, out TEdge successor))
                        break;

                    visited[successor] = 0;
                    Tree(current, successor);
                    if (!TryGetNextInTree(current, out current))
                        break;
                }
            }

            return true;
        }

        private void Colorize( TVertex vertex)
        {
            TVertex current = vertex;
            while (NotInTree(current))
            {
                SetInTree(current);
                if (!TryGetNextInTree(current, out current))
                    break;
            }
        }
    }
}

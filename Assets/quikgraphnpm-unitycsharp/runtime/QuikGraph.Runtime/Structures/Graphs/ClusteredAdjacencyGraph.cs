using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace QuikGraph
{
    /// <summary>
    /// Mutable clustered adjacency graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type</typeparam>

    [Serializable]
    [DebuggerDisplay("VertexCount = {" + nameof(VertexCount) + "}, EdgeCount = {" + nameof(EdgeCount) + "}")]
    public class ClusteredAdjacencyGraph<TVertex, TEdge>
        : IVertexAndEdgeListGraph<TVertex, TEdge>
        , IEdgeListAndIncidenceGraph<TVertex, TEdge>
        , IClusteredGraph
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteredAdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="wrappedGraph">Graph to wrap.</param>
        public ClusteredAdjacencyGraph( AdjacencyGraph<TVertex, TEdge> wrappedGraph)
        {
            Parent = null;
            Wrapped = wrappedGraph ?? throw new ArgumentNullException(nameof(wrappedGraph));
            _clusters = new ArrayList();
            Collapsed = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteredAdjacencyGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="parentGraph">Parent graph.</param>
        public ClusteredAdjacencyGraph( ClusteredAdjacencyGraph<TVertex, TEdge> parentGraph)
        {
            Parent = parentGraph ?? throw new ArgumentNullException(nameof(parentGraph));
            Wrapped = new AdjacencyGraph<TVertex, TEdge>(parentGraph.AllowParallelEdges);
            _clusters = new ArrayList();
        }

        /// <summary>
        /// Parent graph.
        /// </summary>
        
        public ClusteredAdjacencyGraph<TVertex, TEdge> Parent { get; }

        /// <summary>
        /// Wrapped graph.
        /// </summary>
        
        protected AdjacencyGraph<TVertex, TEdge> Wrapped { get; }

        /// <summary>
        /// Gets or sets the edge capacity.
        /// </summary>
        public int EdgeCapacity
        {
            get => Wrapped.EdgeCapacity;
            set => Wrapped.EdgeCapacity = value;
        }

        /// <summary>
        /// Gets the type of vertices.
        /// </summary>
        
        public Type VertexType => typeof(TVertex);

        /// <summary>
        /// Gets the type of edges.
        /// </summary>
        
        public Type EdgeType => typeof(TEdge);

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => Wrapped.IsDirected;

        /// <inheritdoc />
        public bool AllowParallelEdges => Wrapped.AllowParallelEdges;

        #endregion

        #region IClusteredGraph

        /// <inheritdoc />
        public bool Collapsed { get; set; }

        
        private readonly ArrayList _clusters;

        /// <inheritdoc />
        public IEnumerable Clusters => _clusters.Cast<object>();

        /// <inheritdoc />
        public int ClustersCount => _clusters.Count;

        /// <summary>
        /// Adds a new cluster.
        /// </summary>
        /// <returns>The added cluster.</returns>
        public ClusteredAdjacencyGraph<TVertex, TEdge> AddCluster()
        {
            var cluster = new ClusteredAdjacencyGraph<TVertex, TEdge>(this);
            _clusters.Add(cluster);
            return cluster;
        }

        /// <inheritdoc />
        IClusteredGraph IClusteredGraph.AddCluster()
        {
            return AddCluster();
        }

        /// <inheritdoc />
        public void RemoveCluster(IClusteredGraph graph)
        {
            if (graph is null)
                throw new ArgumentNullException(nameof(graph));

            _clusters.Remove(graph);
        }

        #endregion

        #region IVertexSet<TVertex>

        /// <inheritdoc />
        public bool IsVerticesEmpty => Wrapped.IsVerticesEmpty;

        /// <inheritdoc />
        public int VertexCount => Wrapped.VertexCount;

        /// <inheritdoc />
        public virtual IEnumerable<TVertex> Vertices => Wrapped.Vertices;

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return Wrapped.ContainsVertex(vertex);
        }

        #endregion

        #region IEdgeSet<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsEdgesEmpty => Wrapped.IsEdgesEmpty;

        /// <inheritdoc />
        public int EdgeCount => Wrapped.EdgeCount;

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> Edges => Wrapped.Edges;

        /// <inheritdoc />
        public bool ContainsEdge(TEdge edge)
        {
            return Wrapped.ContainsEdge(edge);
        }

        #endregion

        #region IIncidenceGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return Wrapped.ContainsEdge(source, target);
        }

        /// <inheritdoc />
        public bool TryGetEdge(TVertex source, TVertex target, out TEdge edge)
        {
            return Wrapped.TryGetEdge(source, target, out edge);
        }

        /// <inheritdoc />
        public virtual bool TryGetEdges(TVertex source, TVertex target, out IEnumerable<TEdge> edges)
        {
            return Wrapped.TryGetEdges(source, target, out edges);
        }

        #endregion

        #region IImplicitGraph<TVertex,TEdge> 

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return Wrapped.IsOutEdgesEmpty(vertex);
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return Wrapped.OutDegree(vertex);
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            return Wrapped.OutEdges(vertex);
        }

        /// <inheritdoc />
        public virtual bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            return Wrapped.TryGetOutEdges(vertex, out edges);
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return Wrapped.OutEdge(vertex, index);
        }

        #endregion

        /// <summary>
        /// Adds a vertex to this graph.
        /// </summary>
        /// <param name="vertex">Vertex to add.</param>
        /// <returns>True if the vertex was added, false otherwise.</returns>
        public virtual bool AddVertex( TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!(Parent is null || Parent.ContainsVertex(vertex)))
            {
                Parent.AddVertex(vertex);
                return Wrapped.AddVertex(vertex);
            }

            return Wrapped.AddVertex(vertex);
        }

        /// <summary>
        /// Adds given vertices to this graph.
        /// </summary>
        /// <param name="vertices">Vertices to add.</param>
        /// <returns>The number of vertex added.</returns>
        public virtual int AddVertexRange( IEnumerable<TVertex> vertices)
        {
            if (vertices is null)
                throw new ArgumentNullException(nameof(vertices));
            TVertex[] verticesArray = vertices.ToArray();
            if (verticesArray.Any(v => v == null))
                throw new ArgumentNullException(nameof(vertices), "At least one vertex is null.");

            int count = 0;
            foreach (TVertex vertex in verticesArray)
            {
                if (AddVertex(vertex))
                    ++count;
            }

            return count;
        }

        /// <summary>
        /// Removes the given vertex from clusters.
        /// </summary>
        /// <param name="vertex">Vertex to remove.</param>
        private void RemoveChildVertex( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            foreach (ClusteredAdjacencyGraph<TVertex, TEdge> cluster in Clusters)
            {
                if (cluster.ContainsVertex(vertex))
                {
                    cluster.Wrapped.RemoveVertex(vertex);
                    cluster.RemoveChildVertex(vertex);
                }
            }
        }

        /// <summary>
        /// Removes the given vertex from this graph.
        /// </summary>
        /// <param name="vertex">Vertex to remove.</param>
        /// <returns>True if the vertex was removed, false otherwise.</returns>
        public virtual bool RemoveVertex( TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (!Wrapped.ContainsVertex(vertex))
                return false;

            RemoveChildVertex(vertex);
            Wrapped.RemoveVertex(vertex);
            Parent?.RemoveVertex(vertex);

            return true;
        }

        /// <summary>
        /// Removes all vertices matching the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Predicate to check on each vertex.</param>
        /// <returns>The number of vertex removed.</returns>
        public int RemoveVertexIf( VertexPredicate<TVertex> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            TVertex[] verticesToRemove = Vertices
                .Where(vertex => predicate(vertex))
                .ToArray();

            foreach (TVertex vertex in verticesToRemove)
                RemoveVertex(vertex);

            return verticesToRemove.Length;
        }

        /// <summary>
        /// Adds <paramref name="edge"/> and its vertices to this graph.
        /// </summary>
        /// <param name="edge">The edge to add.</param>
        /// <returns>True if the edge was added, false otherwise.</returns>
        public virtual bool AddVerticesAndEdge( TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            AddVertex(edge.Source);
            AddVertex(edge.Target);
            return AddEdge(edge);
        }

        /// <summary>
        /// Adds a set of edges (and it's vertices if necessary).
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges added.</returns>
        public int AddVerticesAndEdgeRange( IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            int count = 0;
            foreach (TEdge edge in edgesArray)
            {
                if (AddVerticesAndEdge(edge))
                    ++count;
            }

            return count;
        }

        /// <summary>
        /// Adds the <paramref name="edge"/> to this graph.
        /// </summary>
        /// <param name="edge">An edge.</param>
        /// <returns>True if the edge was added, false otherwise.</returns>
        public virtual bool AddEdge( TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (Parent != null && !Parent.ContainsEdge(edge))
                Parent.AddEdge(edge);
            return Wrapped.AddEdge(edge);
        }

        /// <summary>
        /// Adds a set of edges to this graph.
        /// </summary>
        /// <param name="edges">Edges to add.</param>
        /// <returns>The number of edges successfully added to this graph.</returns>
        public int AddEdgeRange( IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));
            TEdge[] edgesArray = edges.ToArray();
            if (edgesArray.Any(e => e == null))
                throw new ArgumentNullException(nameof(edges), "At least one edge is null.");

            int count = 0;
            foreach (TEdge edge in edgesArray)
            {
                if (AddEdge(edge))
                    ++count;
            }

            return count;
        }

        private void RemoveChildEdge( TEdge edge)
        {
            Debug.Assert(edge != null);

            foreach (ClusteredAdjacencyGraph<TVertex, TEdge> cluster in Clusters)
            {
                if (cluster.ContainsEdge(edge))
                {
                    cluster.Wrapped.RemoveEdge(edge);
                    cluster.RemoveChildEdge(edge);
                }
            }
        }

        /// <summary>
        /// Removes the <paramref name="edge"/> from this graph.
        /// </summary>
        /// <param name="edge">Edge to remove.</param>
        /// <returns>True if the <paramref name="edge"/> was successfully removed, false otherwise.</returns>
        public virtual bool RemoveEdge( TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (!Wrapped.ContainsEdge(edge))
                return false;

            RemoveChildEdge(edge);
            Wrapped.RemoveEdge(edge);
            Parent?.RemoveEdge(edge);

            return true;
        }

        /// <summary>
        /// Removes all edges that match the given <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">Predicate to check if an edge should be removed.</param>
        /// <returns>The number of edges removed.</returns>
        public int RemoveEdgeIf( EdgePredicate<TVertex, TEdge> predicate)
        {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            var edgesToRemove = Wrapped.Edges
                .Where(edge => predicate(edge))
                .ToArray();

            foreach (TEdge edge in edgesToRemove)
                RemoveEdge(edge);

            return edgesToRemove.Length;
        }

        /// <summary>
        /// Removes all out-edges of the <paramref name="vertex"/>
        /// where the <paramref name="predicate"/> is evaluated to true.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="predicate">Predicate to remove edges.</param>
        /// <returns>The number of removed edges.</returns>
        public int RemoveOutEdgeIf( TVertex vertex,  EdgePredicate<TVertex, TEdge> predicate)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));

            int edgeToRemoveCount = Wrapped.RemoveOutEdgeIf(vertex, predicate);
            Parent?.RemoveOutEdgeIf(vertex, predicate);

            return edgeToRemoveCount;
        }

        /// <summary>
        /// Clears the out-edges of the given <paramref name="vertex"/>
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public void ClearOutEdges( TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            Wrapped.ClearOutEdges(vertex);
        }

        /// <summary>
        /// Clears the vertex and edges.
        /// </summary>
        public void Clear()
        {
            Wrapped.Clear();
            _clusters.Clear();
        }
    }
}

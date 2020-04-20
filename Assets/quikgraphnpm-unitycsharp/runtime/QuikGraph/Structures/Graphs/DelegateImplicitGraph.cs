using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A delegate-based directed implicit graph data structure.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class DelegateImplicitGraph<TVertex, TEdge> : IImplicitGraph<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateImplicitGraph{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="tryGetOutEdges">Getter of out-edges.</param>
        /// <param name="allowParallelEdges">
        /// Indicates if parallel edges are allowed.
        /// Note that get of edges is delegated so you may have bugs related
        /// to parallel edges due to the delegated implementation.
        /// </param>
        public DelegateImplicitGraph(
            [JBNotNull] TryFunc<TVertex, IEnumerable<TEdge>> tryGetOutEdges,
            bool allowParallelEdges = true)
        {
            _tryGetOutEdgesFunc = tryGetOutEdges ?? throw new ArgumentNullException(nameof(tryGetOutEdges));
            AllowParallelEdges = allowParallelEdges;
        }

        /// <summary>
        /// Getter of out-edges.
        /// </summary>
        [JBNotNull]
        private readonly TryFunc<TVertex, IEnumerable<TEdge>> _tryGetOutEdgesFunc;

        #region IGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsDirected => true;

        /// <inheritdoc />
        public bool AllowParallelEdges { get; }

        #endregion

        #region IImplicitGraph<TVertex,TEdge>

        /// <inheritdoc />
        public bool IsOutEdgesEmpty(TVertex vertex)
        {
            return !OutEdges(vertex).Any();
        }

        /// <inheritdoc />
        public int OutDegree(TVertex vertex)
        {
            return OutEdges(vertex).Count();
        }

        [JBPure]
        [JBNotNull, ItemNotNull]
        internal virtual IEnumerable<TEdge> OutEdgesInternal([JBNotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (_tryGetOutEdgesFunc(vertex, out IEnumerable<TEdge> outEdges))
                return outEdges;
            throw new VertexNotFoundException();
        }

        /// <inheritdoc />
        public IEnumerable<TEdge> OutEdges(TVertex vertex)
        {
            return OutEdgesInternal(vertex);
        }

        [JBPure]
        internal virtual bool TryGetOutEdgesInternal([JBNotNull] TVertex vertex, out IEnumerable<TEdge> edges)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetOutEdgesFunc(vertex, out edges);
        }

        /// <inheritdoc />
        public bool TryGetOutEdges(TVertex vertex, out IEnumerable<TEdge> edges)
        {
            return TryGetOutEdgesInternal(vertex, out edges);
        }

        /// <inheritdoc />
        public TEdge OutEdge(TVertex vertex, int index)
        {
            return OutEdges(vertex).ElementAt(index);
        }

        [JBPure]
        internal virtual bool ContainsVertexInternal([JBNotNull] TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _tryGetOutEdgesFunc(vertex, out _);
        }

        /// <inheritdoc />
        public bool ContainsVertex(TVertex vertex)
        {
            return ContainsVertexInternal(vertex);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Condensation
{
    /// <summary>
    /// An edge that merge several other edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class MergedEdge<TVertex, TEdge> : Edge<TVertex>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MergedEdge{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public MergedEdge([JBNotNull] TVertex source, [JBNotNull] TVertex target)
            : base(source, target)
        {
        }

        [JBNotNull, ItemNotNull]
        private List<TEdge> _edges = new List<TEdge>();

        /// <summary>
        /// Merged edges.
        /// </summary>
        [JBNotNull, ItemNotNull]
        public IList<TEdge> Edges => _edges;

        /// <summary>
        /// Merges the given two edges.
        /// </summary>
        /// <param name="inEdge">First edge.</param>
        /// <param name="outEdge">Second edge.</param>
        /// <returns>The merged edge.</returns>
        [JBPure]
        [JBNotNull]
        public static MergedEdge<TVertex, TEdge> Merge(
            [JBNotNull] MergedEdge<TVertex, TEdge> inEdge,
            [JBNotNull] MergedEdge<TVertex, TEdge> outEdge)
        {
            if (inEdge is null)
                throw new ArgumentNullException(nameof(inEdge));
            if (outEdge is null)
                throw new ArgumentNullException(nameof(outEdge));

            var newEdge = new MergedEdge<TVertex, TEdge>(inEdge.Source, outEdge.Target)
            {
                _edges = new List<TEdge>(inEdge.Edges.Count + outEdge.Edges.Count)
            };
            newEdge._edges.AddRange(inEdge._edges);
            newEdge._edges.AddRange(outEdge._edges);

            return newEdge;
        }
    }

    /// <summary>
    /// Helpers for <see cref="Condensation.MergedEdge{TVertex,TEdge}"/>.
    /// </summary>
    public static class MergedEdge
    {
        /// <inheritdoc cref="MergedEdge{TVertex,TEdge}.Merge"/>
        [JBPure]
        [JBNotNull]
        public static MergedEdge<TVertex, TEdge> Merge<TVertex, TEdge>(
            [JBNotNull] MergedEdge<TVertex, TEdge> inEdge,
            [JBNotNull] MergedEdge<TVertex, TEdge> outEdge)
            where TEdge : IEdge<TVertex>
        {
            return MergedEdge<TVertex, TEdge>.Merge(inEdge, outEdge);
        }
    }
}

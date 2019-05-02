﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of edges predecessors.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class EdgePredecessorRecorderObserver<TVertex, TEdge> : IObserver<IEdgePredecessorRecorderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgePredecessorRecorderObserver()
            : this(new Dictionary<TEdge, TEdge>(), new List<TEdge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgePredecessorRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edgesPredecessors">Edges predecessors.</param>
        /// <param name="endPathEdges">Path ending edges.</param>
        public EdgePredecessorRecorderObserver(
            [NotNull] IDictionary<TEdge, TEdge> edgesPredecessors,
            [NotNull, ItemNotNull] ICollection<TEdge> endPathEdges)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(edgesPredecessors != null);
            Contract.Requires(endPathEdges != null);
#endif

            EdgesPredecessors = edgesPredecessors;
            EndPathEdges = endPathEdges;
        }

        /// <summary>
        /// Edges predecessors.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public IDictionary<TEdge, TEdge> EdgesPredecessors { get; }

        /// <summary>
        /// Path ending edges.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull]
        public ICollection<TEdge> EndPathEdges { get; }

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IEdgePredecessorRecorderAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverTreeEdge += OnEdgeDiscovered;
            algorithm.FinishEdge += OnEdgeFinished;

            return Finally(() =>
            {
                algorithm.DiscoverTreeEdge -= OnEdgeDiscovered;
                algorithm.FinishEdge -= OnEdgeFinished;
            });
        }

        #endregion

        /// <summary>
        /// Gets a path starting with <paramref name="startingEdge"/>.
        /// </summary>
        /// <param name="startingEdge">Starting edge.</param>
        /// <returns>Edge path.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        public ICollection<TEdge> Path(TEdge startingEdge)
        {
            var path = new List<TEdge>();

            TEdge currentEdge = startingEdge;
            path.Insert(0, currentEdge);
            while (EdgesPredecessors.TryGetValue(currentEdge, out TEdge edge))
            {
                path.Insert(0, edge);
                currentEdge = edge;
            }

            return path;
        }

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <returns>Enumerable of paths.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        public IEnumerable<ICollection<TEdge>> AllPaths()
        {
            return EndPathEdges.Select(Path);
        }

        /// <summary>
        /// Merges the path starting at <paramref name="startingEdge"/> with remaining edges.
        /// </summary>
        /// <param name="startingEdge">Starting edge.</param>
        /// <param name="colors">Edges colors mapping.</param>
        /// <returns>Merged path.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        public ICollection<TEdge> MergedPath(
            [NotNull] TEdge startingEdge,
            [NotNull] IDictionary<TEdge, GraphColor> colors)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(startingEdge != null);
            Contract.Requires(colors != null);
#endif

            var path = new List<TEdge>();

            TEdge currentEdge = startingEdge;
            GraphColor color = colors[currentEdge];
            if (color != GraphColor.White)
                return path;
            colors[currentEdge] = GraphColor.Black;

            path.Insert(0, currentEdge);
            while (EdgesPredecessors.TryGetValue(currentEdge, out TEdge edge))
            {
                color = colors[edge];
                if (color != GraphColor.White)
                    return path;
                colors[edge] = GraphColor.Black;

                path.Insert(0, edge);
                currentEdge = edge;
            }

            return path;
        }

        /// <summary>
        /// Gets all merged path.
        /// </summary>
        /// <returns>Enumerable of merged paths.</returns>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [JetBrains.Annotations.Pure]
        [NotNull, ItemNotNull]
        public IEnumerable<ICollection<TEdge>> AllMergedPaths()
        {
            var colors = new Dictionary<TEdge, GraphColor>();

            foreach (KeyValuePair<TEdge, TEdge> pair in EdgesPredecessors)
            {
                colors[pair.Key] = GraphColor.White;
                colors[pair.Value] = GraphColor.White;
            }

            return EndPathEdges.Select(edge => MergedPath(edge, colors));
        }

        private void OnEdgeDiscovered([NotNull] TEdge edge, [NotNull] TEdge targetEdge)
        {
            if (!edge.Equals(targetEdge))
                EdgesPredecessors[targetEdge] = edge;
        }

        private void OnEdgeFinished([NotNull] TEdge finishedEdge)
        {
            foreach (TEdge edge in EdgesPredecessors.Values)
            {
                if (finishedEdge.Equals(edge))
                    return;
            }

            EndPathEdges.Add(finishedEdge);
        }
    }
}

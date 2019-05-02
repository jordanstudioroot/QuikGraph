using System;
using System.Collections.Generic;
using System.Linq;
#if SUPPORTS_CONTRACTS
using System.Diagnostics.Contracts;
#endif
using JetBrains.Annotations;
using static QuikGraph.Utils.DisposableHelpers;

namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of encountered vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public sealed class VertexRecorderObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRecorderObserver{TVertex}"/> class.
        /// </summary>
        public VertexRecorderObserver()
            : this(new List<TVertex>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRecorderObserver{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">Set of vertices.</param>
        public VertexRecorderObserver([NotNull, ItemNotNull] IEnumerable<TVertex> vertices)
        {
#if SUPPORTS_CONTRACTS
            Contract.Requires(vertices != null);
#endif

            _vertices = vertices.ToList();
        }

        [NotNull, ItemNotNull]
        private readonly IList<TVertex> _vertices;

        /// <summary>
        /// Encountered vertices.
        /// </summary>
#if SUPPORTS_CONTRACTS
        [System.Diagnostics.Contracts.Pure]
#endif
        [NotNull, ItemNotNull]
        public IEnumerable<TVertex> Vertices => _vertices.AsEnumerable();

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            algorithm.DiscoverVertex += OnVertexDiscovered;
            return Finally(() => algorithm.DiscoverVertex -= OnVertexDiscovered);
        }

        #endregion

        private void OnVertexDiscovered([NotNull] TVertex vertex)
        {
            _vertices.Add(vertex);
        }
    }
}

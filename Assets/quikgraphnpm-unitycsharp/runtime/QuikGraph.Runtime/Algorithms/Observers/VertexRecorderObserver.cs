using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static QuikGraph.Utils.DisposableHelpers;


namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of encountered vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [Serializable]
    public sealed class VertexRecorderObserver<TVertex> : IObserver<IVertexTimeStamperAlgorithm<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRecorderObserver{TVertex}"/> class.
        /// </summary>
        public VertexRecorderObserver()
        {
            _vertices = new List<TVertex>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexRecorderObserver{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">Set of vertices.</param>
        public VertexRecorderObserver( IEnumerable<TVertex> vertices)
        {
            if (vertices is null)
                throw new ArgumentNullException(nameof(vertices));

            _vertices = vertices.ToList();
        }

        
        private readonly IList<TVertex> _vertices;

        /// <summary>
        /// Encountered vertices.
        /// </summary>
        
        public IEnumerable<TVertex> Vertices => _vertices.AsEnumerable();

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(IVertexTimeStamperAlgorithm<TVertex> algorithm)
        {
            if (algorithm is null)
                throw new ArgumentNullException(nameof(algorithm));

            algorithm.DiscoverVertex += OnVertexDiscovered;
            return Finally(() => algorithm.DiscoverVertex -= OnVertexDiscovered);
        }

        #endregion

        private void OnVertexDiscovered( TVertex vertex)
        {
            Debug.Assert(vertex != null);

            _vertices.Add(vertex);
        }
    }
}

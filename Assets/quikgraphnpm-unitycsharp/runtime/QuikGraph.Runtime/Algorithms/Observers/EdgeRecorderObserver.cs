using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static QuikGraph.Utils.DisposableHelpers;


namespace QuikGraph.Algorithms.Observers
{
    /// <summary>
    /// Recorder of encountered edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    [Serializable]
    public sealed class EdgeRecorderObserver<TVertex, TEdge> : IObserver<ITreeBuilderAlgorithm<TVertex, TEdge>>
        where TEdge : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        public EdgeRecorderObserver()
        {
            _edges = new List<TEdge>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeRecorderObserver{TVertex,TEdge}"/> class.
        /// </summary>
        /// <param name="edges">Set of edges.</param>
        public EdgeRecorderObserver( IEnumerable<TEdge> edges)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            _edges = edges.ToList();
        }

        
        private readonly IList<TEdge> _edges;

        /// <summary>
        /// Encountered edges.
        /// </summary>
        
        public IEnumerable<TEdge> Edges => _edges.AsEnumerable();

        #region IObserver<TAlgorithm>

        /// <inheritdoc />
        public IDisposable Attach(ITreeBuilderAlgorithm<TVertex, TEdge> algorithm)
        {
            if (algorithm is null)
                throw new ArgumentNullException(nameof(algorithm));

            algorithm.TreeEdge += OnEdgeDiscovered;
            return Finally(() => algorithm.TreeEdge -= OnEdgeDiscovered);
        }

        #endregion

        private void OnEdgeDiscovered( TEdge edge)
        {
            Debug.Assert(edge != null);

            _edges.Add(edge);
        }
    }
}

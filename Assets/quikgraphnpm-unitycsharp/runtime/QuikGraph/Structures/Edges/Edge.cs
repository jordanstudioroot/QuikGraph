using System;
using System.Diagnostics;
using QuikGraph.Constants;


namespace QuikGraph
{
    /// <summary>
    /// The default <see cref="IEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>

    [Serializable]
    [DebuggerDisplay("{" + nameof(Source) + "}->{" + nameof(Target) + "}")]
    public class Edge<TVertex> : IEdge<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public Edge( TVertex source,  TVertex target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Source = source;
            Target = target;
        }

        /// <inheritdoc />
        public TVertex Source { get; }

        /// <inheritdoc />
        public TVertex Target { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.EdgeFormatString, Source, Target);
        }
    }
}

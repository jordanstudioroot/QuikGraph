using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace QuikGraph
{
    /// <summary>
    /// The default struct based <see cref="IUndirectedEdge{TVertex}"/> implementation.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>

    [Serializable]
    [DebuggerDisplay("{" + nameof(Source) + "}<->{" + nameof(Target) + "}")]
    public class EquatableUndirectedEdge<TVertex> : UndirectedEdge<TVertex>, IEquatable<EquatableUndirectedEdge<TVertex>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquatableUndirectedEdge{TVertex}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        public EquatableUndirectedEdge( TVertex source,  TVertex target)
            : base(source, target)
        {
        }

        /// <inheritdoc />
        public virtual bool Equals(EquatableUndirectedEdge<TVertex> other)
        {
            if (other is null)
                return false;
            return EqualityComparer<TVertex>.Default.Equals(Source, other.Source)
                && EqualityComparer<TVertex>.Default.Equals(Target, other.Target);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as EquatableUndirectedEdge<TVertex>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCodeHelpers.Combine(Source.GetHashCode(), Target.GetHashCode());
        }
    }
}

using System;
using System.Collections.Generic;


namespace QuikGraph.Collections {
    /// <summary>
    /// Stores a list of vertices.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    [Serializable]
    public sealed class VertexList<TVertex> :
        List<TVertex>,
        ICloneable {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="VertexList{TVertex}"/> class.
        /// </summary>
        public VertexList() { }

        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="VertexList{TVertex}"/> class.
        /// </summary>
        /// <param name="capacity">
        ///     List capacity.
        /// </param>
        public VertexList(int capacity) : base(capacity) { }

        /// <inheritdoc />
        public VertexList( VertexList<TVertex> other) : base(other) { }

        /// <summary>
        ///     Clones this vertex list.
        /// </summary>
        /// <returns>
        ///     Cloned list.
        /// </returns>
        
        public VertexList<TVertex> Clone() {
            return new VertexList<TVertex>(this);
        }

        /// <inheritdoc />
        object ICloneable.Clone() {
            return Clone();
        }
    }
}

using System;
using System.Collections.Generic;


namespace QuikGraph.Predicates
{
    /// <summary>
    /// Predicate that tests if a vertex is a vertex map.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TValue">Type of the value associated to vertices.</typeparam>

    [Serializable]
    public sealed class InDictionaryVertexPredicate<TVertex, TValue>
    {
        
        private readonly IDictionary<TVertex, TValue> _vertexMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="InDictionaryVertexPredicate{TVertex,TValue}"/> class.
        /// </summary>
        /// <param name="vertexMap">Vertex map.</param>
        public InDictionaryVertexPredicate( IDictionary<TVertex, TValue> vertexMap)
        {
            _vertexMap = vertexMap ?? throw new ArgumentNullException(nameof(vertexMap));
        }

        /// <summary>
        /// Checks if the given <paramref name="vertex"/> is in the vertex map.
        /// </summary>
        /// <remarks>Check if the implemented predicate is matched.</remarks>
        /// <param name="vertex">Vertex to use in predicate.</param>
        /// <returns>True if the vertex is in the vertex map, false otherwise.</returns>
        
        public bool Test( TVertex vertex)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            return _vertexMap.ContainsKey(vertex);
        }
    }
}

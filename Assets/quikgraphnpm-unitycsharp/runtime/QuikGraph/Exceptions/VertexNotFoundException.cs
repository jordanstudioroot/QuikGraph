
using System;
using System.Runtime.Serialization;


namespace QuikGraph {
/// <summary>
///     Exception raised when trying to use a vertex that is not
///     inside the manipulated graph.
/// </summary>

    [Serializable]
    public class VertexNotFoundException : QuikGraphException {
/// <summary>
///     Initializes a new instance of
///     <see cref="VertexNotFoundException"/> class.
/// </summary>
        public VertexNotFoundException() : base(
            "Vertex is not present in the graph."
        ) { }

/// <summary>
///     Initializes a new instance of
///     <see cref="VertexNotFoundException"/> class.
/// </summary>
        public VertexNotFoundException(
             string message
        ) : base(message) { }


/// <summary>
///     Initializes a new instance of
///     <see cref="VertexNotFoundException"/>
///     with serialized data.
/// </summary>
/// <param name="info">
///     <see cref="SerializationInfo"/> that contains serialized data
///     concerning the thrown exception.
/// </param>
/// <param name="context">
///     <see cref="StreamingContext"/> that contains contextual information.
/// </param>
        protected VertexNotFoundException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}

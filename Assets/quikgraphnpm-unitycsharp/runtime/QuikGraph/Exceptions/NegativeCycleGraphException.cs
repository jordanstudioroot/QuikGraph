
using System;
using System.Runtime.Serialization;

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm detected a negative cycle in a graph.
    /// </summary>

    [Serializable]
    public class NegativeCycleGraphException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> class.
        /// </summary>
        public NegativeCycleGraphException()
            : base("The graph contains at least one negative cycle.")
        {
        }


        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCycleGraphException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NegativeCycleGraphException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

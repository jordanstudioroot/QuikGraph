
using System;
using System.Runtime.Serialization;

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm find a negative capacity in a graph.
    /// </summary>

    [Serializable]
    public class NegativeCapacityException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCapacityException"/> class.
        /// </summary>
        public NegativeCapacityException()
            : base("The graph contains at least one negative capacity.")
        {
        }


        /// <summary>
        /// Initializes a new instance of <see cref="NegativeCapacityException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NegativeCapacityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

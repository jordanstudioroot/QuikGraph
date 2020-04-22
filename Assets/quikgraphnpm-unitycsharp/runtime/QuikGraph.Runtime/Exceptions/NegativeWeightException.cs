
using System;
using System.Runtime.Serialization;

namespace QuikGraph
{
    /// <summary>
    /// Exception raised when an algorithm find or computed a negative weight in a graph.
    /// </summary>

    [Serializable]
    public class NegativeWeightException : QuikGraphException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> class.
        /// </summary>
        public NegativeWeightException()
            : base("The graph contains at least one negative weight.")
        {
        }


        /// <summary>
        /// Initializes a new instance of <see cref="NegativeWeightException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected NegativeWeightException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

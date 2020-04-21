using System;
using System.Runtime.Serialization;

namespace QuikGraph
{
    /// <summary>
    /// QuikGraph base exception.
    /// </summary>

    [Serializable]
    public abstract class QuikGraphException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="QuikGraphException"/> with the given message.
        /// </summary>
        /// <param name="message">Exception message.</param>
        protected QuikGraphException(string message)
            : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of <see cref="QuikGraphException"/> with serialized data.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains serialized data
        /// concerning the thrown exception.</param>
        /// <param name="context"><see cref="StreamingContext"/> that contains contextual information.</param>
        protected QuikGraphException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }
    }
}

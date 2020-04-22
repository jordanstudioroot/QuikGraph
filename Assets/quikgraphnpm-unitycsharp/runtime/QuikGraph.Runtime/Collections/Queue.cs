using System;

namespace QuikGraph.Collections
{
    /// <inheritdoc cref="System.Collections.Generic.Queue{T}" />
    [Serializable]
    public sealed class Queue<T> : System.Collections.Generic.Queue<T>, IQueue<T>
    {
    }
}

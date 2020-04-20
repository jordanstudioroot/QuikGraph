using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using QuikGraph.Constants;

namespace QuikGraph
{
    /// <summary>
    /// The default implementation of an <see cref="IEdge{TVertex}"/> that supports tagging.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TTag">Tag type.</typeparam>
#if SUPPORTS_SERIALIZATION
    [Serializable]
#endif
    public class TaggedEdge<TVertex, TTag> : Edge<TVertex>, ITagged<TTag>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedEdge{TVertex, TTag}"/> class.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="target">The target vertex.</param>
        /// <param name="tag">Edge tag.</param>
        public TaggedEdge([JBNotNull] TVertex source, [JBNotNull] TVertex target, [JBCanBeNull] TTag tag)
            : base(source, target)
        {
            _tag = tag;
        }

        /// <inheritdoc />
        public event EventHandler TagChanged;

        /// <summary>
        /// Event invoker for <see cref="TagChanged"/> event.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        protected virtual void OnTagChanged([JBNotNull] EventArgs args)
        {
            Debug.Assert(args != null);

            TagChanged?.Invoke(this, args);
        }

        private TTag _tag;

        /// <inheritdoc />
        public TTag Tag
        {
            get => _tag;
            set
            {
                if (EqualityComparer<TTag>.Default.Equals(_tag, value))
                    return;

                _tag = value;
                OnTagChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Format(EdgeConstants.TaggedEdgeFormatString, Source, Target, Tag?.ToString() ?? "no tag");
        }
    }
}

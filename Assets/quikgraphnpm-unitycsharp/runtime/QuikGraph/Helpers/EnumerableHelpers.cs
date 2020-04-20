using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// Enumerable helpers.
    /// </summary>
    internal static class EnumerableHelpers
    {
        /// <summary>
        /// Performs an <see cref="Action{T}"/> on each item in an enumerable,
        /// used to shortcut a "foreach" loop.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="elements">Enumerable to iterate through.</param>
        /// <param name="action">Acton to perform on each element.</param>
        public static void ForEach<T>(
            [JBNotNull, ItemCanBeNull] this IEnumerable<T> elements, 
            [JBNotNull, JBInstantHandle] Action<T> action)
        {
            Debug.Assert(elements != null);
            Debug.Assert(action != null);

            foreach (T elem in elements)
            {
                action(elem);
            }
        }
    }
}
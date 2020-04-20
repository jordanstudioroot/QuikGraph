using System.Collections.Generic;
using JetBrains.Annotations;

namespace QuikGraph
{
    /// <summary>
    /// A directed graph with vertices of type <typeparamref name="TVertex"/>
    /// and terminal edges of type <typeparamref name="TEdge"/>, that is efficient
    /// to traverse both in and out edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface ITermBidirectionalGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
        where TEdge : ITermEdge<TVertex>
    {
        /// <summary>
        /// Gets the number of out terminals on the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Number of out terminals.</returns>
        [JBPure]
        int OutTerminalCount([JBNotNull] TVertex vertex);

        /// <summary>
        /// Checks if the requested out terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>True if the out terminal is empty, false otherwise.</returns>
        [JBPure]
        bool IsOutEdgesEmptyAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out degree on terminal <paramref name="terminal"/>.</returns>
        [JBPure]
        int OutDegreeAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out-edges on terminal <paramref name="terminal"/>.</returns>
        [JBPure]
        [JBNotNull, ItemNotNull]
        IEnumerable<TEdge> OutEdgesAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> out-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">Out-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and out-edges were found, false otherwise.</returns>
        [JBPure]
        [JBContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetOutEdgesAt([JBNotNull] TVertex vertex, int terminal, [ItemNotNull] out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the number of in terminals on the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Number of in terminals.</returns>
        [JBPure]
        int InTerminalCount([JBNotNull] TVertex vertex);

        /// <summary>
        /// Checks if the requested in terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>True if the in terminal is empty, false otherwise.</returns>
        [JBPure]
        bool IsInEdgesEmptyAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in degree on terminal <paramref name="terminal"/>.</returns>
        [JBPure]
        int InDegreeAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in-edges on terminal <paramref name="terminal"/>.</returns>
        [JBPure]
        [JBNotNull, ItemNotNull]
        IEnumerable<TEdge> InEdgesAt([JBNotNull] TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">In-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and in-edges were found, false otherwise.</returns>
        [JBPure]
        [JBContractAnnotation("=> true, edges:notnull;=> false, edges:null")]
        bool TryGetInEdgesAt([JBNotNull] TVertex vertex, int terminal, [ItemNotNull] out IEnumerable<TEdge> edges);
    }
}
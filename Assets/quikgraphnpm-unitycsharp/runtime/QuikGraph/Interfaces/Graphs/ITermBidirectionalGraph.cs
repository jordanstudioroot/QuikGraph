using System.Collections.Generic;


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
        
        int OutTerminalCount( TVertex vertex);

        /// <summary>
        /// Checks if the requested out terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>True if the out terminal is empty, false otherwise.</returns>
        
        bool IsOutEdgesEmptyAt( TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out degree on terminal <paramref name="terminal"/>.</returns>
        
        int OutDegreeAt( TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> out edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <returns>The <paramref name="vertex"/> out-edges on terminal <paramref name="terminal"/>.</returns>
        
        
        IEnumerable<TEdge> OutEdgesAt( TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> out-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">Out-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and out-edges were found, false otherwise.</returns>        
        bool TryGetOutEdgesAt( TVertex vertex, int terminal,  out IEnumerable<TEdge> edges);

        /// <summary>
        /// Gets the number of in terminals on the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>Number of in terminals.</returns>
        
        int InTerminalCount( TVertex vertex);

        /// <summary>
        /// Checks if the requested in terminal is empty or not for the given <paramref name="vertex"/>.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>True if the in terminal is empty, false otherwise.</returns>
        
        bool IsInEdgesEmptyAt( TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in degree for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in degree on terminal <paramref name="terminal"/>.</returns>
        
        int InDegreeAt( TVertex vertex, int terminal);

        /// <summary>
        /// Gets the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">In terminal index.</param>
        /// <returns>The <paramref name="vertex"/> in-edges on terminal <paramref name="terminal"/>.</returns>
        
        
        IEnumerable<TEdge> InEdgesAt( TVertex vertex, int terminal);

        /// <summary>
        /// Tries to get the <paramref name="vertex"/> in-edges for the requested terminal.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <param name="terminal">Out terminal index.</param>
        /// <param name="edges">In-edges found, otherwise null.</param>
        /// <returns>True if <paramref name="vertex"/> was found or/and in-edges were found, false otherwise.</returns>        
        bool TryGetInEdgesAt( TVertex vertex, int terminal,  out IEnumerable<TEdge> edges);
    }
}
using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Assignment
{
    /// <summary>
    /// State of an iteration of the Hungarian algorithm.
    /// </summary>
    public struct HungarianIteration
    {
        /// <summary>
        /// Costs matrix.
        /// </summary>
        [JBNotNull]
        public int[,] Matrix { get; }

        /// <summary>
        /// Matrix mask.
        /// </summary>
        [JBNotNull]
        public byte[,] Mask { get; }

        /// <summary>
        /// Array of treated rows.
        /// </summary>
        [JBNotNull]
        public bool[] RowsCovered { get; }

        /// <summary>
        /// Array of treated columns.
        /// </summary>
        [JBNotNull]
        public bool[] ColumnsCovered { get; }

        /// <summary>
        /// <see cref="HungarianAlgorithm.Steps"/> corresponding to this iteration.
        /// </summary>
        public HungarianAlgorithm.Steps Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HungarianIteration"/> struct.
        /// </summary>
        internal HungarianIteration(
            [JBNotNull] int[,] costs,
            [JBNotNull] byte[,] mask,
            [JBNotNull] bool[] rowsCovered,
            [JBNotNull] bool[] columnsCovered, 
            HungarianAlgorithm.Steps step)
        {
            Matrix = costs;
            Mask = mask;
            RowsCovered = rowsCovered;
            ColumnsCovered = columnsCovered;
            Step = step;
        }
    }
}

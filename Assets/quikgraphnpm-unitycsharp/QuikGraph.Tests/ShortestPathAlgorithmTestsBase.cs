using System;
using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms.ShortestPath;

namespace QuikGraph.Tests.Algorithms.ShortestPath
{
    /// <summary>
    /// Base class for shortest path algorithms.
    /// </summary>
    internal abstract class ShortestPathAlgorithmTestsBase : RootedAlgorithmTestsBase
    {
        #region Test helpers

        protected static void TryGetDistance_Test<TEdge, TGraph>(
             ShortestPathAlgorithmBase<int, TEdge, TGraph> algorithm)
            where TEdge : IEdge<int>
            where TGraph : IVertexSet<int>
        {
            const int vertex1 = 1;

            algorithm.Compute(vertex1);
            Assert.IsTrue(algorithm.TryGetDistance(vertex1, out double distance));
            Assert.AreEqual(algorithm.Distances[vertex1], distance);

            const int vertex2 = 2;
            Assert.IsFalse(algorithm.TryGetDistance(vertex2, out _));
        }

        protected static void TryGetDistance_Throws_Test<TVertex, TEdge, TGraph>(
             ShortestPathAlgorithmBase<TVertex, TEdge, TGraph> algorithm)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
            where TGraph : IVertexSet<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.TryGetDistance(null, out _));

            var vertex = new TVertex();
            Assert.Throws<InvalidOperationException>(() => algorithm.TryGetDistance(vertex, out _));
        }

        protected static void TryGetDistance_Test<TEdge>(
             UndirectedShortestPathAlgorithmBase<int, TEdge> algorithm)
            where TEdge : IEdge<int>
        {
            const int vertex1 = 1;

            algorithm.Compute(vertex1);
            Assert.IsTrue(algorithm.TryGetDistance(vertex1, out double distance));
            Assert.AreEqual(algorithm.Distances[vertex1], distance);

            const int vertex2 = 2;
            Assert.IsFalse(algorithm.TryGetDistance(vertex2, out _));
        }

        protected static void TryGetDistance_Throws_Test<TVertex, TEdge>(
             UndirectedShortestPathAlgorithmBase<TVertex, TEdge> algorithm)
            where TVertex : class, new()
            where TEdge : IEdge<TVertex>
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => algorithm.TryGetDistance(null, out _));

            var vertex = new TVertex();
            Assert.Throws<InvalidOperationException>(() => algorithm.TryGetDistance(vertex, out _));
        }

        #endregion
    }
}

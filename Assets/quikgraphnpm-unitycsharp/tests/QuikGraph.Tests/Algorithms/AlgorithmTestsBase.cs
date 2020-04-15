using JetBrains.Annotations;
using NUnit.Framework;
using QuikGraph.Algorithms;

namespace QuikGraph.Tests.Algorithms
{
    /// <summary>
    /// Base class for algorithm tests.
    /// </summary>
    internal abstract class AlgorithmTestsBase
    {
        #region Test helpers

        protected static void AssertAlgorithmState<TGraph>(
            [NotNull] AlgorithmBase<TGraph> algorithm,
            [NotNull] TGraph treatedGraph,
            ComputationState state = ComputationState.NotRunning)
        {
            Assert.IsNotNull(treatedGraph);
            Assert.AreSame(treatedGraph, algorithm.VisitedGraph);
            Assert.IsNotNull(algorithm.Services);
            Assert.IsNotNull(algorithm.SyncRoot);
            Assert.AreEqual(state, algorithm.State);
        }

        #endregion
    }
}

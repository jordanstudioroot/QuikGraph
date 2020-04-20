using JetBrains.Annotations;

namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Represents algorithm component (services).
    /// </summary>
    public interface IAlgorithmComponent
    {
        /// <summary>
        /// Algorithm common services.
        /// </summary>
        [JBNotNull]
        IAlgorithmServices Services { get; }

        /// <summary>
        /// Gets the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <returns>Found service, otherwise null.</returns>
        [JBPure]
        [JBCanBeNull]
        T GetService<T>();

        /// <summary>
        /// Tries to get the service with given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Found service.</param>
        /// <returns>True if the service was found, false otherwise.</returns>
        [JBPure]
        [JBContractAnnotation("=> true, service:notnull;=> false, service:null")]
        bool TryGetService<T>(out T service);
    }
}

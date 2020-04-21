
using System;


namespace QuikGraph.Algorithms.Services
{
    /// <summary>
    /// Default algorithm services implementation.
    /// </summary>
    internal class AlgorithmServices : IAlgorithmServices
    {
        
        private readonly IAlgorithmComponent _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlgorithmServices"/> class.
        /// </summary>
        /// <param name="host">Algorithm host.</param>
        public AlgorithmServices( IAlgorithmComponent host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        private ICancelManager _cancelManager;

        /// <inheritdoc />
        public ICancelManager CancelManager => 
            (_cancelManager ?? (_cancelManager = _host.GetService<ICancelManager>())) ?? throw new InvalidOperationException("No cancel manager service registered.");
    }
}

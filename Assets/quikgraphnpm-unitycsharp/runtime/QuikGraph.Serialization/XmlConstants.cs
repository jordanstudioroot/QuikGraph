using JetBrains.Annotations;

namespace QuikGraph.Serialization
{
    internal static class XmlConstants
    {
        [JBNotNull]
        public const string DynamicMethodPrefix = "QuikGraph";

        #region Tags

        [JBNotNull]
        public const string GraphMLTag = "graphml";

        [JBNotNull]
        public const string GraphTag = "graph";

        [JBNotNull]
        public const string NodeTag = "node";

        [JBNotNull]
        public const string EdgeTag = "edge";

        [JBNotNull]
        public const string DataTag = "data";

        #endregion

        #region Attributes

        [JBNotNull]
        public const string IdAttribute = "id";

        [JBNotNull]
        public const string SourceAttribute = "source";

        [JBNotNull]
        public const string TargetAttribute = "target";

        #endregion
    }
}
using System;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.XPath;


namespace QuikGraph.Serialization
{
    /// <summary>
    /// Extensions to serialize/deserialize graphs.
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Serializes the <paramref name="graph"/> to the <paramref name="stream"/> using the .NET serialization binary formatter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <param name="graph">The graph to serialize.</param>
        /// <param name="stream">Stream in which serializing the graph.</param>
        public static void SerializeToBinary<TVertex, TEdge>(
             this IGraph<TVertex, TEdge> graph,
             Stream stream)
            where TEdge : IEdge<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanWrite)
                throw new ArgumentException("Must be a writable stream", nameof(stream));

            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, graph);
        }

        /// <summary>
        /// Deserializes a graph instance from a <paramref name="stream"/> that was serialized using the .NET serialization binary formatter.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="stream">Stream from which deserializing the graph.</param>
        /// <returns>Deserialized graph.</returns>
        
        public static TGraph DeserializeFromBinary<TVertex, TEdge, TGraph>( this Stream stream)
            where TGraph : IGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead)
                throw new ArgumentException("Must be a readable stream", nameof(stream));

            var formatter = new BinaryFormatter();
            object result = formatter.Deserialize(stream);
            return (TGraph)result;
        }

        
        private static TGraph DeserializeFromXmlInternal<TVertex, TEdge, TGraph>(
             XmlReader reader,
             Predicate<XmlReader> graphPredicate,
             Predicate<XmlReader> vertexPredicate,
             Predicate<XmlReader> edgePredicate,
             Func<XmlReader, TGraph> graphFactory,
             Func<XmlReader, TVertex> vertexFactory,
             Func<XmlReader, TEdge> edgeFactory)
            where TGraph : class, IMutableVertexAndEdgeSet<TVertex, TEdge> where TEdge : IEdge<TVertex>
        {
            // Find the graph node
            TGraph graph = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && graphPredicate(reader))
                {
                    graph = graphFactory(reader);
                    break;
                }
            }

            if (graph is null)
                throw new InvalidOperationException("Could not find graph node.");

            using (XmlReader graphReader = reader.ReadSubtree())
            {
                while (graphReader.Read())
                {
                    if (graphReader.NodeType == XmlNodeType.Element)
                    {
                        if (vertexPredicate(graphReader))
                        {
                            TVertex vertex = vertexFactory(graphReader);
                            graph.AddVertex(vertex);
                        }
                        else if (edgePredicate(reader))
                        {
                            TEdge edge = edgeFactory(graphReader);
                            graph.AddEdge(edge);
                        }
                    }
                }
            }

            return graph;
        }

        private static TGraph DeserializeFromXmlInternal<TVertex, TEdge, TGraph>(
             this IXPathNavigable document,
             string graphXPath,
             string vertexXPath,
             string edgeXPath,
             Func<XPathNavigator, TGraph> graphFactory,
             Func<XPathNavigator, TVertex> vertexFactory,
             Func<XPathNavigator, TEdge> edgeFactory)
            where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            XPathNavigator navigator = document.CreateNavigator();
            XPathNavigator graphNode = navigator?.SelectSingleNode(graphXPath);
            if (graphNode is null)
                throw new InvalidOperationException("Could not find graph node.");
            TGraph graph = graphFactory(graphNode);
            foreach (XPathNavigator vertexNode in graphNode.Select(vertexXPath))
            {
                TVertex vertex = vertexFactory(vertexNode);
                graph.AddVertex(vertex);
            }

            foreach (XPathNavigator edgeNode in graphNode.Select(edgeXPath))
            {
                TEdge edge = edgeFactory(edgeNode);
                graph.AddEdge(edge);
            }

            return graph;
        }

        /// <summary>
        /// Deserializes a graph instance from a generic XML stream, using an <see cref="XPathDocument"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="document">Input XML document.</param>
        /// <param name="graphXPath">XPath expression to the graph node. The first node is considered.</param>
        /// <param name="vertexXPath">XPath expression from the graph node to the vertex nodes.</param>
        /// <param name="edgeXPath">XPath expression from the graph node to the edge nodes.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">
        /// If the <paramref name="document"/> does not allow to get an XML navigator
        /// or if the <paramref name="graphXPath"/> does not allow to get graph node.
        /// </exception>
        
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
             this IXPathNavigable document,
             string graphXPath,
             string vertexXPath,
             string edgeXPath,
             Func<XPathNavigator, TGraph> graphFactory,
             Func<XPathNavigator, TVertex> vertexFactory,
             Func<XPathNavigator, TEdge> edgeFactory)
            where TGraph : IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            if (document is null)
                throw new ArgumentNullException(nameof(document));
            if (string.IsNullOrEmpty(graphXPath))
                throw new ArgumentException("Graph path cannot be null or empty", nameof(graphXPath));
            if (string.IsNullOrEmpty(vertexXPath))
                throw new ArgumentException("Vertex path cannot be null or empty", nameof(vertexXPath));
            if (string.IsNullOrEmpty(edgeXPath))
                throw new ArgumentException("Edge path cannot be null or empty", nameof(edgeXPath));
            if (graphFactory is null)
                throw new ArgumentNullException(nameof(graphFactory));
            if (vertexFactory is null)
                throw new ArgumentNullException(nameof(vertexFactory));
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            return DeserializeFromXmlInternal(
                document,
                graphXPath,
                vertexXPath,
                edgeXPath,
                graphFactory,
                vertexFactory,
                edgeFactory);
        }

        /// <summary>
        /// Deserializes a graph instance from a generic XML stream, using an <see cref="XmlReader"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="reader">Input XML reader.</param>
        /// <param name="graphPredicate">Predicate that returns a value indicating if the current XML node is a graph. The first match is considered.</param>
        /// <param name="vertexPredicate">Predicate that returns a value indicating if the current XML node is a vertex.</param>
        /// <param name="edgePredicate">Predicate that returns a value indicating if the current XML node is an edge.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">If the graph node cannot be found.</exception>
        
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
             this XmlReader reader,
             Predicate<XmlReader> graphPredicate,
             Predicate<XmlReader> vertexPredicate,
             Predicate<XmlReader> edgePredicate,
             Func<XmlReader, TGraph> graphFactory,
             Func<XmlReader, TVertex> vertexFactory,
             Func<XmlReader, TEdge> edgeFactory)
            where TGraph : class, IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (graphPredicate is null)
                throw new ArgumentNullException(nameof(graphPredicate));
            if (vertexPredicate is null)
                throw new ArgumentNullException(nameof(vertexPredicate));
            if (edgePredicate is null)
                throw new ArgumentNullException(nameof(edgePredicate));
            if (graphFactory is null)
                throw new ArgumentNullException(nameof(graphFactory));
            if (vertexFactory is null)
                throw new ArgumentNullException(nameof(vertexFactory));
            if (edgeFactory is null)
                throw new ArgumentNullException(nameof(edgeFactory));

            return DeserializeFromXmlInternal(reader, graphPredicate, vertexPredicate, edgePredicate, graphFactory, vertexFactory, edgeFactory);
        }

        /// <summary>
        /// Deserializes a graph from a generic XML stream, using an <see cref="XmlReader"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="reader">Input XML reader.</param>
        /// <param name="namespaceUri">XML namespace.</param>
        /// <param name="graphElementName">Name of the XML node holding graph information. The first node is considered.</param>
        /// <param name="vertexElementName">Name of the XML node holding vertex information.</param>
        /// <param name="edgeElementName">Name of the XML node holding edge information.</param>
        /// <param name="graphFactory">Delegate that instantiates the empty graph instance, given the graph node.</param>
        /// <param name="vertexFactory">Delegate that instantiates a vertex instance, given the vertex node.</param>
        /// <param name="edgeFactory">Delegate that instantiates an edge instance, given the edge node.</param>
        /// <returns>Deserialized graph.</returns>
        /// <exception cref="InvalidOperationException">If the graph node cannot be found.</exception>
        
        public static TGraph DeserializeFromXml<TVertex, TEdge, TGraph>(
             this XmlReader reader,
             string graphElementName,
             string vertexElementName,
             string edgeElementName,
             string namespaceUri,
             Func<XmlReader, TGraph> graphFactory,
             Func<XmlReader, TVertex> vertexFactory,
             Func<XmlReader, TEdge> edgeFactory)
            where TGraph : class, IMutableVertexAndEdgeSet<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            if (string.IsNullOrEmpty(graphElementName))
                throw new ArgumentException($"{nameof(graphElementName)} cannot be null or empty.", nameof(graphElementName));
            if (string.IsNullOrEmpty(vertexElementName))
                throw new ArgumentException($"{nameof(vertexElementName)} cannot be null or empty.", nameof(vertexElementName));
            if (string.IsNullOrEmpty(edgeElementName))
                throw new ArgumentException($"{nameof(edgeElementName)} cannot be null or empty.", nameof(edgeElementName));
            if (namespaceUri is null)
                throw new ArgumentNullException(nameof(namespaceUri));

            return DeserializeFromXml(
                reader,
                r => r.Name == graphElementName && r.NamespaceURI == namespaceUri,
                r => r.Name == vertexElementName && r.NamespaceURI == namespaceUri,
                r => r.Name == edgeElementName && r.NamespaceURI == namespaceUri,
                graphFactory,
                vertexFactory,
                edgeFactory);
        }

        /// <summary>
        /// Serializes a graph instance to a generic XML stream, using an <see cref="XmlWriter"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="graph">Graph to serialize.</param>
        /// <param name="writer">XML writer.</param>
        /// <param name="vertexIdentity">The vertex identity.</param>
        /// <param name="edgeIdentity">The edge identity.</param>
        /// <param name="graphElementName">Name of the graph element.</param>
        /// <param name="vertexElementName">Name of the vertex element.</param>
        /// <param name="edgeElementName">Name of the edge element.</param>
        /// <param name="namespaceUri">XML namespace.</param>
        public static void SerializeToXml<TVertex, TEdge, TGraph>(
             this TGraph graph,
             XmlWriter writer,
             VertexIdentity<TVertex> vertexIdentity,
             EdgeIdentity<TVertex, TEdge> edgeIdentity,
             string graphElementName,
             string vertexElementName,
             string edgeElementName,
             string namespaceUri)
            where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            SerializeToXml(
                graph,
                writer,
                vertexIdentity,
                edgeIdentity,
                graphElementName,
                vertexElementName,
                edgeElementName,
                namespaceUri,
                null,
                null,
                null);
        }

        /// <summary>
        /// Serializes a graph instance to a generic XML stream, using an <see cref="XmlWriter"/>.
        /// </summary>
        /// <typeparam name="TVertex">Vertex type.</typeparam>
        /// <typeparam name="TEdge">Edge type.</typeparam>
        /// <typeparam name="TGraph">Graph type.</typeparam>
        /// <param name="graph">Graph to serialize.</param>
        /// <param name="writer">XML writer.</param>
        /// <param name="vertexIdentity">The vertex identity.</param>
        /// <param name="edgeIdentity">The edge identity.</param>
        /// <param name="graphElementName">Name of the graph element.</param>
        /// <param name="vertexElementName">Name of the vertex element.</param>
        /// <param name="edgeElementName">Name of the edge element.</param>
        /// <param name="namespaceUri">XMl namespace.</param>
        /// <param name="writeGraphAttributes">Delegate to write graph attributes (optional).</param>
        /// <param name="writeVertexAttributes">Delegate to write vertex attributes (optional).</param>
        /// <param name="writeEdgeAttributes">Delegate to write edge attributes (optional).</param>
        public static void SerializeToXml<TVertex, TEdge, TGraph>(
             this TGraph graph,
             XmlWriter writer,
             VertexIdentity<TVertex> vertexIdentity,
             EdgeIdentity<TVertex, TEdge> edgeIdentity,
             string graphElementName,
             string vertexElementName,
             string edgeElementName,
             string namespaceUri,
             Action<XmlWriter, TGraph> writeGraphAttributes,
             Action<XmlWriter, TVertex> writeVertexAttributes,
             Action<XmlWriter, TEdge> writeEdgeAttributes)
            where TGraph : IVertexAndEdgeListGraph<TVertex, TEdge>
            where TEdge : IEdge<TVertex>
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (vertexIdentity is null)
                throw new ArgumentNullException(nameof(vertexIdentity));
            if (edgeIdentity is null)
                throw new ArgumentNullException(nameof(edgeIdentity));
            if (string.IsNullOrEmpty(graphElementName))
                throw new ArgumentException($"{nameof(graphElementName)} cannot be null or empty.", nameof(graphElementName));
            if (string.IsNullOrEmpty(vertexElementName))
                throw new ArgumentException($"{nameof(vertexElementName)} cannot be null or empty.", nameof(vertexElementName));
            if (string.IsNullOrEmpty(edgeElementName))
                throw new ArgumentException($"{nameof(edgeElementName)} cannot be null or empty.", nameof(edgeElementName));
            if (namespaceUri is null)
                throw new ArgumentNullException(nameof(namespaceUri));

            writer.WriteStartElement(graphElementName, namespaceUri);

            writeGraphAttributes?.Invoke(writer, graph);

            foreach (TVertex vertex in graph.Vertices)
            {
                writer.WriteStartElement(vertexElementName, namespaceUri);
                writer.WriteAttributeString("id", namespaceUri, vertexIdentity(vertex));
                writeVertexAttributes?.Invoke(writer, vertex);
                writer.WriteEndElement();
            }

            foreach (TEdge edge in graph.Edges)
            {
                writer.WriteStartElement(edgeElementName, namespaceUri);
                writer.WriteAttributeString("id", namespaceUri, edgeIdentity(edge));
                writer.WriteAttributeString("source", namespaceUri, vertexIdentity(edge.Source));
                writer.WriteAttributeString("target", namespaceUri, vertexIdentity(edge.Target));
                writeEdgeAttributes?.Invoke(writer, edge);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}

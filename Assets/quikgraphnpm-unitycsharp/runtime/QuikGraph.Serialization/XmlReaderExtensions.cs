using System;
using System.Xml;


namespace QuikGraph.Serialization
{
    /// <summary>
    /// Extensions for <see cref="XmlReader"/> to help deserializing graph data.
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Reads the content of a named element as an array of booleans.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Boolean array.</returns>
        
        
        public static bool[] ReadElementContentAsBooleanArray(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToBoolean);
        }

        /// <summary>
        /// Reads the content of a named element as an array of ints.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Int array.</returns>
        
        
        public static int[] ReadElementContentAsInt32Array(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToInt32);
        }

        /// <summary>
        /// Reads the content of a named element as an array of longs.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Long array.</returns>
        
        
        public static long[] ReadElementContentAsInt64Array(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToInt64);
        }

        /// <summary>
        /// Reads the content of a named element as an array of floats.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Float array.</returns>
        
        
        public static float[] ReadElementContentAsSingleArray(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToSingle);
        }

        /// <summary>
        /// Reads the content of a named element as an array of doubles.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>Double array.</returns>
        
        
        public static double[] ReadElementContentAsDoubleArray(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, Convert.ToDouble);
        }

        /// <summary>
        /// Reads the content of a named element as an array of strings.
        /// </summary>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <returns>String array.</returns>
        
        
        public static string[] ReadElementContentAsStringArray(
             XmlReader xmlReader,
             string localName,
             string namespaceURI)
        {
            return ReadElementContentAsArray(xmlReader, localName, namespaceURI, str => str);
        }

        /// <summary>
        /// Read contents of an XML element as an array of type T.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="xmlReader">XML reader.</param>
        /// <param name="localName">Node name.</param>
        /// <param name="namespaceURI">XML namespace.</param>
        /// <param name="stringToT">Converts the XML element string as <typeparamref name="T"/>.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        
        
        public static T[] ReadElementContentAsArray<T>(
             XmlReader xmlReader,
             string localName,
             string namespaceURI,
             Func<string, T> stringToT)
        {
            string str = xmlReader.ReadElementContentAsString(localName, namespaceURI);
            if (str == "null")
                return null;

            if (str.Length > 0 && str[str.Length - 1] == ' ')
            {
                str = str.Remove(str.Length - 1);
            }

            string[] strArray = str.Split(' ');

            var array = new T[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                array[i] = stringToT(strArray[i]);
            }

            return array;
        }
    }
}
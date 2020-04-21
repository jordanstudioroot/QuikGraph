using System.Collections.Generic;
using System.Xml;



namespace QuikGraph.Serialization
{
    /// <summary>
    /// Extensions for <see cref="XmlWriter"/> to help serializing graph data.
    /// </summary>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of booleans.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteBooleanArray( XmlWriter xmlWriter,  IList<bool> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of ints.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteInt32Array( XmlWriter xmlWriter,  IList<int> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of longs.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteInt64Array( XmlWriter xmlWriter,  IList<long> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of floats.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteSingleArray( XmlWriter xmlWriter,  IList<float> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of doubles.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteDoubleArray( XmlWriter xmlWriter,  IList<double> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes the content of <paramref name="value"/> as an array of strings.
        /// </summary>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">Array to serialize.</param>
        public static void WriteStringArray( XmlWriter xmlWriter,  IList<string> value)
        {
            WriteArray(xmlWriter, value);
        }

        /// <summary>
        /// Writes an array as space separated values. There is a space after every value, even the last one.
        /// If array is null, it writes "null".
        /// If array is empty, it writes empty string.
        /// If array is a string array with only one element "null", then it writes "null ".
        /// </summary>
        /// <typeparam name="T">Element value.</typeparam>
        /// <param name="xmlWriter">XML writer.</param>
        /// <param name="value">List of values to write.</param>
        public static void WriteArray<T>( XmlWriter xmlWriter,  IList<T> value)
        {
            if (value is null)
            {
                xmlWriter.WriteString("null");
                return;
            }

            var strArray = new string[value.Count];
            for (int i = 0; i < value.Count; ++i)
            {
                strArray[i] = value[i].ToString();
            }

            var str = string.Join(" ", strArray);
            str += " ";
            xmlWriter.WriteString(str);
        }
    }
}
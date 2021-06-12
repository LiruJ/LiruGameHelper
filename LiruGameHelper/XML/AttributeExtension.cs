using System;
using System.Collections.Generic;
using System.Xml;

namespace LiruGameHelper.XML
{
    public static class AttributeExtension
    {
        #region Delegates
        public delegate bool TryParse<T>(string input, out T output);
        #endregion

        public static void AddAttribute(this XmlNode node, string name, object value)
        {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
            attribute.Value = value.ToString();
            node.Attributes.Append(attribute);
        }

        public static void AddAttributeList<T>(this XmlNode node, string name, IReadOnlyCollection<T> values, char separator = ',')
        {
            XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);

            string listString = "";
            foreach (T value in values)
                listString += value.ToString() + separator + ' ';
            
            attribute.Value = listString.Length == 0 ? listString : listString.Remove(listString.Length - 2);
            node.Attributes.Append(attribute);
        }

        public static T ParseAttributeValue<T>(this XmlNode node, string attributeName, Func<string, T> parser)
        {
            // Get the string value.
            string value = node.GetAttributeValue(attributeName);

            // Parse the value with the supplied parser.
            return parser(value);
        }

        public static bool TryParseAttributeValue<T>(this XmlNode node, string attributeName, TryParse<T> parser, out T output)
        {
            // If the parser is null, throw an exception.
            if (parser == null) throw new ArgumentNullException(nameof(parser));

            // Try get the string value.
            if (!GetAttributeValue(node, attributeName, out string input)) { output = default; return false; }

            // Try parse the string value and return the result.
            return parser(input, out output);
        }

        public static bool ParseAttributeValueOrDefault<T>(this XmlNode node, string attributeName, TryParse<T> parser, out T output, T defaultTo)
        {
            if (!node.TryParseAttributeValue(attributeName, parser, out output))
            {
                output = defaultTo;
                return false;
            }
            else return true;
        }

        public static string GetAttributeValue(this XmlNode node, string attributeName)
            => (node == null || !node.GetAttributeValue(attributeName, out string value)) ? throw new FormatException($"{node?.Name}'s {attributeName} could not be parsed.") : value.Replace(@"\n", Environment.NewLine);

        public static bool GetAttributeValue(this XmlNode node, string attributeName, out string value)
        {
            // If the given node is null, throw an exception.
            if (node == null) throw new ArgumentNullException("node", "Given XMLNode cannot be null.");

            // Get the attribute from the node.
            XmlNode nodeAttribute = node.Attributes.GetNamedItem(attributeName);

            // If the attribute does not exist, set the out string to empty and return false.
            if (nodeAttribute == null)
            {
                value = string.Empty;
                return false;
            }
            // Otherwise, set the out string to the attribute and return true.
            else
            {
                value = nodeAttribute.Value;
                return true;
            }
        }

        public static bool GetAttributeList(this XmlNode node, string attributeName, out string[] values, char separator = ',')
        {
            // If the given node is null, throw an exception.
            if (node is null) throw new ArgumentNullException("node", "Given XMLNode cannot be null.");

            // Get the attribute from the node, if it does not exist then return false.
            if (!GetAttributeValue(node, attributeName, out string listValue) || string.IsNullOrWhiteSpace(listValue))
            {
                values = null;
                return false;
            }

            // Split the attribute value.
            values = listValue.Split(separator);

            // Trim whitespace.
            for (int i = 0; i < values.Length; i++)
                 values[i] = values[i].Trim();

            // Return true.
            return true;
        }

        public static T[] ParseAttributeListValue<T>(this XmlNode node, string attributeName, Func<string, T> parser, char separator = ',')
        {
            // Try to get a list of the values in string form, if it cannot be done then return false.
            if (!node.GetAttributeList(attributeName, out string[] stringValues, separator))
                return null;

            // Parse each value.
            T[] values = new T[stringValues.Length];
            for (int i = 0; i < stringValues.Length; i++)
                values[i] = parser(stringValues[i]);

            // Return the list.
            return values;
        }

        public static bool TryParseAttributeListValue<T>(this XmlNode node, string attributeName, TryParse<T> parser, out T[] values, out int failedIndex, char separator = ',')
        {
            // Try to get a list of the values in string form, if it cannot be done then return false.
            if (!node.GetAttributeList(attributeName, out string[] stringValues, separator))
            {
                values = null;
                failedIndex = -1;
                return false;
            }

            // Try parse each value.
            values = new T[stringValues.Length];
            for (failedIndex = 0; failedIndex < stringValues.Length; failedIndex++)
                if (!parser(stringValues[failedIndex], out T value)) { return false; }
                else values[failedIndex] = value;

            // Return true as the parse was successful.
            return true;
        }
    }
}

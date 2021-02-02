using System;
using System.Xml;

namespace LiruGameHelper.XML
{
    public static class AttributeExtension
    {
        #region Delegates
        public delegate bool TryParse<T>(string input, out T output);
        #endregion

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

            // Try parse the string value.
            if (!parser(input, out output)) return false;

            // Return the parsed value.
            return true;
        }

        public static T ParseAttributeValueOrDefault<T>(this XmlNode node, string attributeName, TryParse<T> parser, out T output, T defaultTo) 
            => node.TryParseAttributeValue<T>(attributeName, parser, out output) ? output : defaultTo;

        public static string GetAttributeValue(this XmlNode node, string attributeName)
            => (node == null || !node.GetAttributeValue(attributeName, out string value) || string.IsNullOrWhiteSpace(value)) ? throw new FormatException($"{node?.Name}'s {attributeName} could not be parsed.") : value;

        public static bool GetAttributeValue(this XmlNode node, string attributeName, out string value)
        {
            // If the given node is null, throw an exception.
            if (node is null) throw new ArgumentNullException("node", "Given XMLNode cannot be null.");

            // Get the attribute from the node.
            var nodeAttribute = node.Attributes.GetNamedItem(attributeName);

            // If the attribute does not exist, set the out string to empty and return false.
            if (nodeAttribute is null)
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
    }
}

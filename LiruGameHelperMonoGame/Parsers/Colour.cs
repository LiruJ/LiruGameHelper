using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Reflection;

namespace LiruGameHelperMonoGame.Parsers
{
    public static class Colour
    {
        #region Constants
        private static Color defaultColour = Color.Transparent;

        private const char hexChar = '#';

        private const char rgbChar = '*';
        #endregion

        #region Public Parse Functions
        /// <summary> Parses the given <paramref name="input"/> into a <see cref="Color"/>. </summary>
        /// <param name="input"> The input string. </param>
        /// <returns> The parsed <see cref="Color"/>. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> is empty or null. </exception>
        /// <exception cref="FormatException"> <paramref name="input"/> had an invalid format. </exception>
        public static Color Parse(string input)
        {
            // Try to parse the colour, with exceptions being thrown.
            tryParse(input, out Color colour, true);

            // Return the colour.
            return colour;
        }

        /// <summary> Attempts to parse the given <paramref name="input"/> into the output <paramref name="colour"/>, returning a boolean value representing the outcome of the operation. </summary>
        /// <param name="input"> The input string. </param>
        /// <param name="colour"> The output <see cref="Color"/>, or <see cref="Color.Transparent"/> if the parse operation failed. </param>
        /// <returns> <c>true</c> if the parse operation was successful; otherwise, <c>false</c>. </returns>
        public static bool TryParse(string input, out Color colour) => tryParse(input, out colour, false);
        #endregion

        #region Private Parse Functions
        private static bool tryParse(string input, out Color colour, bool throwException = false)
        {
            // If the string is empty or null, throw an exception or return false.
            if (string.IsNullOrWhiteSpace(input)) { colour = defaultColour; return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false; }
           
            // Trim any whitespace.
            input = input.Trim();

            switch (input[0])
            {
                case hexChar:
                    return tryParseHex(input, out colour, throwException);
                case rgbChar:
                    return tryParseRGBA(input, out colour, throwException);
                default:
                    // Get the colour propert from the Monogame Color class.
                    PropertyInfo colourPropertyInfo = typeof(Color).GetProperty(input, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);

                    // If the property could not be found, fail, otherwise; return true along with the colour.
                    if (colourPropertyInfo == null) { colour = defaultColour; return throwException ? throw new FormatException("Given string could not be parsed and does not have an associated preset colour.") : false; }
                    else if (colourPropertyInfo.GetValue(null) is Color presetColour) { colour = presetColour; return true; }
                    else { colour = defaultColour; return throwException ? throw new FormatException("Given string could not be parsed and does not have an associated preset colour.") : false; }
            }
        }

        private static bool tryParseHex(string input, out Color colour, bool throwException = false)
        {
            // Remove the # from the input.
            input = input.TrimStart(hexChar);

            // Start with the default colour.
            colour = defaultColour;

            // Is true if the length of the input signifies that it has an alpha value.
            bool hasAlpha = input.Length % 4 == 0;

            // Find the length of each value within the string.
            int valueLength;
            if (input.Length != 0 && hasAlpha) valueLength = input.Length / 4;
            else if (input.Length != 0 && input.Length % 3 == 0) valueLength = input.Length / 3;
            else return throwException ? throw new FormatException("Hex value must be #rgb, #rrggbb, #rgba, or #rrggbbaa format.") : false;

            // Calculate the maximum value that could be made using the number of digits within the value length.
            int maxValue = (int)Math.Pow(16, valueLength) - 1;

            // Parse the RGB values.
            if (!int.TryParse(input.Substring(0, valueLength), NumberStyles.HexNumber, ParserSettings.FormatProvider, out int red)) return throwException ? throw new FormatException("Red was invalid, must be a valid hex byte.") : false;
            if (!int.TryParse(input.Substring(valueLength, valueLength), NumberStyles.HexNumber, ParserSettings.FormatProvider, out int green)) return throwException ? throw new FormatException("Green was invalid, must be a valid hex byte.") : false;
            if (!int.TryParse(input.Substring(valueLength * 2, valueLength), NumberStyles.HexNumber, ParserSettings.FormatProvider, out int blue)) return throwException ? throw new FormatException("Blue was invalid, must be a valid hex byte.") : false;

            // Parse the alpha if one was given, otherwise default to the max value.
            int alpha;
            if (!hasAlpha) alpha = maxValue;
            else if (!int.TryParse(input.Substring(valueLength * 3, valueLength), NumberStyles.HexNumber, ParserSettings.FormatProvider, out alpha)) return throwException ? throw new FormatException("Alpha was invalid, must be a valid hex byte.") : false;

            // Create the colour and return true.
            colour = new Color((float)red / maxValue, (float)green / maxValue, (float)blue / maxValue, (float)alpha / maxValue);
            return true;
        }

        private static bool tryParseRGBA(string input, out Color colour, bool throwException = false)
        {
            // Remove the * from the input.
            input = input.TrimStart(rgbChar);

            // Split the input string by commas.
            string[] rgbaInputs = input.Split(ParserSettings.Separator);
            string alphaInput = (rgbaInputs.Length >= 4) ? rgbaInputs[3] : byte.MaxValue.ToString();

            // If the input string has three or more inputs, parse it.
            if (rgbaInputs.Length >= 3)
            {
                // Parse the red, green, blue, and alpha into a colour, then return true.
                if (!byte.TryParse(rgbaInputs[0], NumberStyles.Integer, ParserSettings.FormatProvider, out byte red))    { colour = defaultColour; return throwException ? throw new FormatException("Red was invalid, must be a valid byte.")   : false; }
                if (!byte.TryParse(rgbaInputs[1], NumberStyles.Integer, ParserSettings.FormatProvider, out byte green))  { colour = defaultColour; return throwException ? throw new FormatException("Green was invalid, must be a valid byte.") : false; }
                if (!byte.TryParse(rgbaInputs[2], NumberStyles.Integer, ParserSettings.FormatProvider, out byte blue))   { colour = defaultColour; return throwException ? throw new FormatException("Blue was invalid, must be a valid byte.")  : false; }
                if (!byte.TryParse(alphaInput, NumberStyles.Integer, ParserSettings.FormatProvider, out byte alpha))  { colour = defaultColour; return throwException ? throw new FormatException("Alpha was invalid, must be a valid byte.") : false; }
                colour = new Color(red, green, blue, alpha);
                return true;
            }

            // Otherwise, throw an error or return false.
            else { colour = defaultColour; return throwException ? throw new FormatException("RGB colour must be in \"r, g, b\", or \"r, g, b, a\" format.") : false; }
        }
        #endregion
    }
}

using System;
using System.Globalization;
using System.Numerics;

namespace LiruGameHelper.Parsers
{
    public static class ToVector
    {
        #region Public Parse Functions
        public static bool TryParse(string input, out Vector2 vector) => tryParse(input, out vector);

        public static bool TryParse(string input, out Vector3 vector) => tryParse(input, out vector);

        public static Vector2 Parse2(string input)
        {
            // Try to parse the vector, with exceptions being thrown.
            tryParse(input, out Vector2 vector, true);
            
            // Return the vector.
            return vector;
        }

        public static Vector3 Parse3(string input)
        {
            // Try to parse the vector, with exceptions being thrown.
            tryParse(input, out Vector3 vector, true);

            // Return the vector.
            return vector;
        }
        #endregion

        #region Private Parse Functions
        private static bool tryParse(string input, out Vector2 vector, bool throwException = false)
        {
            // If the string is empty or null, throw an exception or return false.
            vector = default;
            if (string.IsNullOrWhiteSpace(input)) 
                return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false;

            // Trim any whitespace.
            input = input.Trim();

            // Split the input into the separate values.
            string[] pointAxes = input.Split(ParserSettings.Separator);

            // Handle the length.
            switch (pointAxes.Length)
            {
                case 1:
                    bool vParsed = float.TryParse(pointAxes[0], NumberStyles.Float, ParserSettings.FormatProvider, out float v);
                    vector = vParsed ? new Vector2(v) : default;
                    return throwException && !vParsed ? throw new ArgumentException($"Could not parse {pointAxes[0]} into a float for vector.") : vParsed;
                case 2:
                    bool xParsed = float.TryParse(pointAxes[0], NumberStyles.Float, ParserSettings.FormatProvider, out float x);
                    bool yParsed = float.TryParse(pointAxes[1], NumberStyles.Float, ParserSettings.FormatProvider, out float y);
                    vector = xParsed && yParsed ? new Vector2(x, y) : default;
                    return throwException && !(xParsed && yParsed) ? throw new ArgumentException($"Could not parse {input} into a vector.") : xParsed && yParsed;
                default:
                    throw new Exception("Vector had an invalid number of components.");
            }
        }

        private static bool tryParse(string input, out Vector3 vector, bool throwException = false)
        {
            // If the string is empty or null, throw an exception or return false.
            vector = default;
            if (string.IsNullOrWhiteSpace(input)) 
                return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false;

            // Trim any whitespace.
            input = input.Trim();

            // Split the input into the separate values.
            string[] pointAxes = input.Split(ParserSettings.Separator);

            // Handle the length.
            switch (pointAxes.Length)
            {
                case 1:
                    bool vParsed = float.TryParse(pointAxes[0], NumberStyles.Float, ParserSettings.FormatProvider, out float v);
                    vector = vParsed ? new Vector3(v) : default;
                    return throwException && !vParsed ? throw new ArgumentException($"Could not parse {pointAxes[0]} into a float for vector.") : vParsed;
                case 3:
                    bool xParsed = float.TryParse(pointAxes[0], NumberStyles.Float, ParserSettings.FormatProvider, out float x);
                    bool yParsed = float.TryParse(pointAxes[1], NumberStyles.Float, ParserSettings.FormatProvider, out float y);
                    bool zParsed = float.TryParse(pointAxes[2], NumberStyles.Float, ParserSettings.FormatProvider, out float z);
                    vector = xParsed && yParsed && zParsed ? new Vector3(x, y, z) : default;
                    return throwException && !(xParsed && yParsed && zParsed) ? throw new ArgumentException($"Could not parse {input} into a vector.") : xParsed && yParsed && zParsed;
                default:
                    throw new Exception("Vector had an invalid number of components.");
            }
        }
        #endregion
    }
}
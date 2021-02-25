using Microsoft.Xna.Framework;
using System;

namespace LiruGameHelperMonoGame.Parsers
{
    public static class ToVector3
    {
        #region Constants
        private static Vector3 defaultVector = Vector3.Zero;

        private const char splitChar = ',';
        #endregion

        #region Public Parse Functions
        public static bool TryParse(string input, out Vector3 vector) => tryParse(input, out vector);

        public static Vector3 Parse(string input)
        {
            // Try to parse the vector, with exceptions being thrown.
            tryParse(input, out Vector3 vector, true);

            // Return the vector.
            return vector;
        }
        #endregion

        #region Private Parse Functions
        private static bool tryParse(string input, out Vector3 vector, bool throwException = false)
        {
            // If the string is empty or null, throw an exception or return false.
            if (string.IsNullOrWhiteSpace(input)) { vector = defaultVector; return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false; }

            // Trim any whitespace.
            input = input.Trim();

            // Split the input into the separate values.
            string[] pointAxes = input.Split(splitChar);

            // Handle the length.
            switch (pointAxes.Length)
            {
                case 1:
                    bool vParsed = float.TryParse(pointAxes[0], out float v);
                    vector = vParsed ? new Vector3(v) : defaultVector;
                    return throwException && !vParsed ? throw new ArgumentException($"Could not parse {pointAxes[0]} into a float for vector.") : vParsed;
                case 3:
                    bool xParsed = float.TryParse(pointAxes[0], out float x);
                    bool yParsed = float.TryParse(pointAxes[1], out float y);
                    bool zParsed = float.TryParse(pointAxes[2], out float z);
                    vector = xParsed && yParsed && zParsed ? new Vector3(x, y, z) : defaultVector;
                    return throwException && !(xParsed && yParsed && zParsed) ? throw new ArgumentException($"Could not parse {input} into a vector.") : xParsed && yParsed && zParsed;
                default:
                    throw new Exception("Vector had an invalid number of components.");
            }
        }
        #endregion
    }
}

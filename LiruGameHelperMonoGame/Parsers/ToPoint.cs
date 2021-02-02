using Microsoft.Xna.Framework;
using System;

namespace LiruGameHelperMonoGame.Parsers
{
    public static class ToPoint
    {
        #region Constants
        private const char separator = ',';
        #endregion

        public static Point Parse(string input)
        {
            // Try to parse with exceptions being thrown.
            tryParse(input, out Point point, true);

            // Return the parsed point.
            return point;
        }

        public static bool TryParse(string input, out Point point) => tryParse(input, out point);

        private static bool tryParse(string input, out Point point, bool throwException = false)
        {
            // If the input is invalid, handle it.
            if (string.IsNullOrWhiteSpace(input)) { point = Point.Zero; return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false; }

            // Split the value.
            string[] pointAxes = input.Split(separator);

            // Parse the first axis first.
            if (!int.TryParse(pointAxes[0], out int xValue)) { point = Point.Zero; return throwException ? throw new ArgumentException($"Invalid x value of point, should be int, was actually {pointAxes[0]}") : false; }

            // If there is only one axis defined, create the point with both axes having that value, otherwise; parse the y value.
            if (pointAxes.Length == 1) { point = new Point(xValue); return true; }
            else
            {
                // Parse the y value.
                if (!int.TryParse(pointAxes[1], out int yValue)) { point = Point.Zero; return throwException ? throw new ArgumentException($"Invalid y value of point, should be int, was actually {pointAxes[1]}") : false; }

                // Create a point with the x and y, then return true.
                point = new Point(xValue, yValue);
                return true;
            }
        }
    }
}

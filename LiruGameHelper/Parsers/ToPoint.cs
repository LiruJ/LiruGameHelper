using System;
using System.Drawing;
using System.Globalization;

namespace LiruGameHelper.Parsers
{
    public static class ToPoint
    {
        #region Public Parse Functions
        public static bool TryParse(string input, out Point point) => tryParse(input, out point);

        public static Point Parse(string input)
        {
            // Try to parse with exceptions being thrown.
            tryParse(input, out Point point, true);

            // Return the parsed point.
            return point;
        }
        #endregion

        #region Private Parse Functions
        private static bool tryParse(string input, out Point point, bool throwException = false)
        {
            // If the input is invalid, handle it.
            point = default;
            if (string.IsNullOrWhiteSpace(input)) 
                return throwException ? throw new ArgumentNullException(nameof(input), "Given string cannot be null, empty, or whitespace.") : false;

            // Split the value.
            string[] pointAxes = input.Split(ParserSettings.Separator);

            // Parse the first axis first.
            if (!int.TryParse(pointAxes[0], NumberStyles.Integer, ParserSettings.FormatProvider, out int xValue)) 
                return throwException ? throw new ArgumentException($"Invalid x value of point, should be int, was actually {pointAxes[0]}") : false; 

            // If there is only one axis defined, create the point with both axes having that value, otherwise; parse the y value.
            if (pointAxes.Length == 1) 
            { 
                point = new Point(xValue);
                return true; 
            }
            else
            {
                // Parse the y value.
                if (!int.TryParse(pointAxes[1], NumberStyles.Integer, ParserSettings.FormatProvider, out int yValue)) 
                    return throwException ? throw new ArgumentException($"Invalid y value of point, should be int, was actually {pointAxes[1]}") : false;

                // Create a point with the x and y, then return true.
                point = new Point(xValue, yValue);
                return true;
            }
        }
        #endregion
    }
}

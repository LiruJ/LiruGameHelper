using Microsoft.Xna.Framework;
using System;

namespace LiruGameHelperMonoGame.Parsers
{
    /// <summary> Allows for comma=separated inputs to be parsed as a <see cref="Rectangle"/>. </summary>
    public static class ToRectangle
    {
        #region Constants
        private const char splitChar = ',';
        #endregion

        #region Public Parse Functions
        /// <summary> Parses the given <paramref name="input"/> into a <see cref="Rectangle"/>. </summary>
        /// <param name="input"> The input string. </param>
        /// <returns> The parsed <see cref="Rectangle"/>. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> is empty or null. </exception>
        /// <exception cref="FormatException"> <paramref name="input"/> had an invalid format. </exception>
        public static Rectangle Parse(string input)
        {
            // Try to parse the rectangle, with exceptions being thrown.
            tryParse(input, out Rectangle rectangle, true);

            // Return the rectangle.
            return rectangle;
        }

        /// <summary> Attempts to parse the given <paramref name="input"/> into the output <paramref name="rectangle"/>, returning a boolean value representing the outcome of the operation. </summary>
        /// <param name="input"> The input string. </param>
        /// <param name="rectangle"> The output <see cref="Rectangle"/>, or <see cref="Rectangle.Empty"/> if the parse operation failed. </param>
        /// <returns> <c>true</c> if the parse operation was successful; otherwise, <c>false</c>. </returns>
        public static bool TryParse(string input, out Rectangle rectangle) => tryParse(input, out rectangle, false);
        #endregion

        #region Private Parse Functions
        private static bool tryParse(string input, out Rectangle rectangle, bool throwException)
        {
            // If the string is null or empty, throw an exception or return false.
            if (string.IsNullOrWhiteSpace(input)) { rectangle = Rectangle.Empty; return throwException ? throw new ArgumentException("Given string cannot be empty.", nameof(input)) : false; }

            // Split the input string by commas.
            string[] splitInput = input.Split(splitChar);

            // If there are not enough inputs, throw an exception or return false.
            if (splitInput.Length < 4) { rectangle = Rectangle.Empty; return throwException ? throw new FormatException("Not enough inputs were given, rectangle must have X, Y, width, and height.") : false; }

            // If any of the inputs fail to parse, throw an error or return false.
            if (!int.TryParse(splitInput[0], out int x)) { rectangle = Rectangle.Empty; return throwException ? throw new FormatException("X was invalid, must be a valid int.") : false; } 
            if (!int.TryParse(splitInput[1], out int y)) { rectangle = Rectangle.Empty; return throwException ? throw new FormatException("Y was invalid, must be a valid int.") : false; }
            if (!int.TryParse(splitInput[2], out int w)) { rectangle = Rectangle.Empty; return throwException ? throw new FormatException("Width was invalid, must be a valid int.") : false; }
            if (!int.TryParse(splitInput[3], out int h)) { rectangle = Rectangle.Empty; return throwException ? throw new FormatException("Height was invalid, must be a valid int.") : false; }

            // Create a rectangle out of the parsed sides and return true.
            rectangle = new Rectangle(x, y, w, h);
            return true;
        }
        #endregion
    }
}

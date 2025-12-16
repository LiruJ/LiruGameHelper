using Microsoft.Xna.Framework;
using System;

namespace LiruGameHelper.MonoGame.Parsers;

/// <summary> Allows for comma=separated inputs to be parsed as a <see cref="Rectangle"/>. </summary>
public static class ToRectangle
{
    #region Public Parse Functions
    /// <summary> Attempts to parse the given <paramref name="input"/> into the output <paramref name="rectangle"/>, returning a boolean value representing the outcome of the operation. </summary>
    /// <param name="input"> The input string. </param>
    /// <param name="rectangle"> The output <see cref="Rectangle"/>, or <see cref="Rectangle.Empty"/> if the parse operation failed. </param>
    /// <returns> <c>true</c> if the parse operation was successful; otherwise, <c>false</c>. </returns>
    public static bool TryParse(string input, out Rectangle rectangle)
    {
        bool parsed = LiruGameHelper.Parsers.ToRectangle.TryParse(input, out System.Drawing.Rectangle drawingRectangle);
        rectangle = parsed ? new Rectangle(drawingRectangle.X, drawingRectangle.Y, drawingRectangle.Width, drawingRectangle.Height) : default;
        return parsed;
    }

    /// <summary> Parses the given <paramref name="input"/> into a <see cref="Rectangle"/>. </summary>
    /// <param name="input"> The input string. </param>
    /// <returns> The parsed <see cref="Rectangle"/>. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="input"/> is empty or null. </exception>
    /// <exception cref="FormatException"> <paramref name="input"/> had an invalid format. </exception>
    public static Rectangle Parse(string input)
    {
        System.Drawing.Rectangle drawingRectangle = LiruGameHelper.Parsers.ToRectangle.Parse(input);
        return new Rectangle(drawingRectangle.X, drawingRectangle.Y, drawingRectangle.Width, drawingRectangle.Height);
    }
    #endregion
}
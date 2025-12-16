using Microsoft.Xna.Framework;
using System;

namespace LiruGameHelper.MonoGame.Parsers;

public static class Colour
{
    #region Public Parse Functions
    /// <summary> Parses the given <paramref name="input"/> into a <see cref="Color"/>. </summary>
    /// <param name="input"> The input string. </param>
    /// <returns> The parsed <see cref="Color"/>. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="input"/> is empty or null. </exception>
    /// <exception cref="FormatException"> <paramref name="input"/> had an invalid format. </exception>
    public static Color Parse(string input)
    {
        System.Drawing.Color drawingColour = LiruGameHelper.Parsers.Colour.Parse(input);
        return new Color(drawingColour.R, drawingColour.G, drawingColour.B, drawingColour.A);
    }

    /// <summary> Attempts to parse the given <paramref name="input"/> into the output <paramref name="colour"/>, returning a boolean value representing the outcome of the operation. </summary>
    /// <param name="input"> The input string. </param>
    /// <param name="colour"> The output <see cref="Color"/>, or <see cref="Color.Transparent"/> if the parse operation failed. </param>
    /// <returns> <c>true</c> if the parse operation was successful; otherwise, <c>false</c>. </returns>
    public static bool TryParse(string input, out Color colour)// => tryParse(input, out colour, false);
    {
        bool parsed = LiruGameHelper.Parsers.Colour.TryParse(input, out System.Drawing.Color drawingColour);
        colour = parsed ? new Color(drawingColour.R, drawingColour.G, drawingColour.B, drawingColour.A) : default;
        return parsed;
    }
    #endregion
}
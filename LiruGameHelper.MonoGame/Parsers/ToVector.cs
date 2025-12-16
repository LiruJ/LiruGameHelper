using Microsoft.Xna.Framework;

namespace LiruGameHelper.MonoGame.Parsers;

public static class ToVector
{
    #region Public Parse Functions
    public static bool TryParse(string input, out Vector2 vector)
    {
        bool parsed = LiruGameHelper.Parsers.ToVector.TryParse(input, out System.Numerics.Vector2 numericsVector);
        vector = parsed ? new Vector2(numericsVector.X, numericsVector.Y) : default;
        return parsed;
    }

    public static bool TryParse(string input, out Vector3 vector)
    {
        bool parsed = LiruGameHelper.Parsers.ToVector.TryParse(input, out System.Numerics.Vector3 numericsVector);
        vector = parsed ? new Vector3(numericsVector.X, numericsVector.Y, numericsVector.Z) : default;
        return parsed;
    }

    public static Vector2 Parse2(string input)
    {
        System.Numerics.Vector2 numericsVector = LiruGameHelper.Parsers.ToVector.Parse2(input);
        return new Vector2(numericsVector.X, numericsVector.Y);
    }

    public static Vector3 Parse3(string input)
    {
        System.Numerics.Vector3 numericsVector = LiruGameHelper.Parsers.ToVector.Parse3(input);
        return new Vector3(numericsVector.X, numericsVector.Y, numericsVector.Z);
    }
    #endregion
}
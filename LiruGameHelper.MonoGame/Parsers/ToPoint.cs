using Microsoft.Xna.Framework;

namespace LiruGameHelper.MonoGame.Parsers
{
    public static class ToPoint
    {
        #region Public Parse Functions
        public static bool TryParse(string input, out Point point)
        {
            bool parsed = LiruGameHelper.Parsers.ToPoint.TryParse(input, out System.Drawing.Point drawingPoint);
            point = parsed ? new Point(drawingPoint.X, drawingPoint.Y) : default;
            return parsed;
        }

        public static Point Parse(string input)
        {
            System.Drawing.Point drawingPoint = LiruGameHelper.Parsers.ToPoint.Parse(input);
            return new Point(drawingPoint.X, drawingPoint.Y);
        }
        #endregion
    }
}
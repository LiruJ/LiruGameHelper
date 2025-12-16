using System;
using System.Drawing;

namespace LiruGameHelper.Helpers;

/// <summary> Helper class for adjusting sizes for aspect ratios. </summary>
public static class AspectRatioHelpers
{
    /// <summary> Adjusts the given <paramref name="size"/> so that the height stays the same and the width is adjusted based on the height and the given <paramref name="ratio"/>. </summary>
    /// <param name="size"> The size to adjust. </param>
    /// <param name="ratio"> The desired ratio. </param>
    /// <param name="allowedDeviance"> The allowed deviation of the adjustment. If the adjustment amount is lower than this value, this function returns <c>false</c>. </param>
    /// <returns> <c>true</c> if the adjustment was a success; otherwise <c>false</c> if the adjustment is lower than <paramref name="allowedDeviance"/>. </returns>
    public static bool AdjustWidth(ref Point size, float ratio, float allowedDeviance)
    {
        // Calculate the adjusted dimension.
        float adjustedWidth = size.Y * ratio;

        // If the dimension is within the correct aspect ratio, do nothing.
        if (MathF.Abs(adjustedWidth - size.X) <= allowedDeviance) return false;

        // Change the size.
        size.X = (int)MathF.Ceiling(adjustedWidth);

        // Return true, as the size was adjusted.
        return true;
    }

    /// <summary> Adjusts the given <paramref name="size"/> so that the width stays the same and the height is adjusted based on the width and the given <paramref name="ratio"/>. </summary>
    /// <param name="size"> The size to adjust. </param>
    /// <param name="ratio"> The desired ratio. </param>
    /// <param name="allowedDeviance"> The allowed deviation of the adjustment. If the adjustment amount is lower than this value, this function returns <c>false</c>. </param>
    /// <returns> <c>true</c> if the adjustment was a success; otherwise <c>false</c> if the adjustment is lower than <paramref name="allowedDeviance"/>. </returns>
    public static bool AdjustHeight(ref Point size, float ratio, float allowedDeviance)
    {
        // Calculate the adjusted dimension.
        float adjustedHeight = size.X / ratio;

        // If the dimension is within the correct aspect ratio, do nothing.
        if (MathF.Abs(adjustedHeight - size.Y) <= allowedDeviance) return false;

        // Change the size.
        size.Y = (int)MathF.Ceiling(adjustedHeight);

        // Return true, as the size was adjusted.
        return true;
    }

    /// <summary> Adjust the given <paramref name="size"/> so that it fits into the given <paramref name="parentSize"/>, based on the given <paramref name="ratio"/>. </summary>
    /// <param name="size"> The size to adjust. </param>
    /// <param name="parentSize"> The size of the parent to fit into. </param>
    /// <param name="ratio"> The desired ratio. This is compared against the ratio of the given <paramref name="parentSize"/> to determine how to fit the size into it. </param>
    /// <param name="allowedDeviance"> The allowed deviation of the adjustment. If the adjustment amount is lower than this value, this function returns <c>false</c>. </param>
    /// <returns> <c>true</c> if the adjustment was a success; otherwise <c>false</c> if the adjustment is lower than <paramref name="allowedDeviance"/>. </returns>
    public static bool FitInBounds(ref Point size, Point parentSize, float ratio, float allowedDeviance)
    {
        // Calculate the ratio of the parent.
        float parentRatio = (float)parentSize.X / parentSize.Y;

        // If the ratio of this size is bigger than that of the parent, use the width.
        if (ratio > parentRatio)
        {
            size.X = parentSize.X;
            return AdjustHeight(ref size, ratio, allowedDeviance);
        }
        // Otherwise; use the height.
        else
        {
            size.Y = parentSize.Y;
            return AdjustWidth(ref size, ratio, allowedDeviance);
        }
    }

    /// <summary> Adjust the given <paramref name="size"/> so that it fits around the given <paramref name="parentSize"/>, based on the given <paramref name="ratio"/>. </summary>
    /// <param name="size"> The size to adjust. </param>
    /// <param name="parentSize"> The size of the parent to fit around. </param>
    /// <param name="ratio"> The desired ratio. This is compared against the ratio of the given <paramref name="parentSize"/> to determine how to fit the size around it. </param>
    /// <param name="allowedDeviance"> The allowed deviation of the adjustment. If the adjustment amount is lower than this value, this function returns <c>false</c>. </param>
    /// <returns> <c>true</c> if the adjustment was a success; otherwise <c>false</c> if the adjustment is lower than <paramref name="allowedDeviance"/>. </returns>
    public static bool FitAroundBounds(ref Point size, Point parentSize, float ratio, float allowedDeviance)
    {
        // Calculate the ratio of the parent.
        float parentRatio = (float)parentSize.X / parentSize.Y;

        // If the ratio of this size is bigger than that of the parent, use the height.
        if (ratio > parentRatio)
        {
            size.Y = parentSize.Y;
            return AdjustWidth(ref size, ratio, allowedDeviance);
        }
        // Otherwise; use the width.
        else
        {
            size.X = parentSize.X;
            return AdjustHeight(ref size, ratio, allowedDeviance);
        }
    }
}
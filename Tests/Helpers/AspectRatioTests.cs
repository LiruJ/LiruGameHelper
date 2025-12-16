using LiruGameHelper.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Tests.Helpers;

[TestClass]
public class AspectRatioTests
{
    private float ratio = 16f / 9f;

    [TestMethod]
    public void DeviationTest()
    {
        // Ensure that it cannot be adjusted with a ratio of 0.5, as no changes can be made.
        Point size = new(1000, 2000);
        Assert.IsFalse(AspectRatioHelpers.AdjustHeight(ref size, 0.5f, 3f), "Height was adjusted when it did not need to be.");
        size = new Point(1000, 2000);
        Assert.IsFalse(AspectRatioHelpers.AdjustWidth(ref size, 0.5f, 3f), "Width was adjusted when it did not need to be.");

        // Ensure it cannot be adjusted again, but with the deviation taken into account.
        size = new Point(1000, 2003);
        Assert.IsFalse(AspectRatioHelpers.AdjustHeight(ref size, 0.5f, 3f), "Height was adjusted when it did not need to be.");
        size = new Point(1003, 2000);
        Assert.IsFalse(AspectRatioHelpers.AdjustWidth(ref size, 0.5f, 3f), "Width was adjusted when it did not need to be.");

        // Ensure it adjusts when it's just out of the deviation zone.
        size = new Point(1000, 2003);
        Assert.IsTrue(AspectRatioHelpers.AdjustHeight(ref size, 0.5f, 2f), "Height was not adjusted when it needed to be.");
        size = new Point(1003, 2000);
        Assert.IsTrue(AspectRatioHelpers.AdjustWidth(ref size, 0.5f, 2f), "Width was not adjusted when it needed to be.");
    }

    [TestMethod]
    public void WidthTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Adjust the width.
        Assert.IsTrue(AspectRatioHelpers.AdjustWidth(ref size, ratio, 2f), "Width was not adjusted.");

        // Ensure the width is correct.
        Assert.AreEqual(1778, size.X, "Adjusted width was incorrect.");
    }

    [TestMethod]
    public void HeightTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Adjust the height.
        Assert.IsTrue(AspectRatioHelpers.AdjustHeight(ref size, ratio, 2f), "Height was not adjusted.");

        // Ensure the height is correct.
        Assert.AreEqual(563, size.Y, "Adjusted height was incorrect.");
    }

    [TestMethod]
    public void FitWidthInBoundsTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Create a parent that is not as wide as the main size.
        Point thinParent = new(500, 1000);

        // Fit the size into the parent size.
        Assert.IsTrue(AspectRatioHelpers.FitInBounds(ref size, thinParent, ratio, 2f), "Could not fit into parent bounds.");

        // Ensure the size is correct.
        Assert.AreEqual(thinParent.X, size.X, "Adjusted width was incorrect.");
        Assert.AreEqual(282, size.Y, "Adjusted height was incorrect.");
    }

    [TestMethod]
    public void FitHeightInBoundsTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Create a parent that is not as tall as the main size.
        Point shortParent = new(1000, 500);

        // Fit the size into the parent size.
        Assert.IsTrue(AspectRatioHelpers.FitInBounds(ref size, shortParent, ratio, 2f), "Could not fit into parent bounds.");

        // Ensure the size is correct.
        Assert.AreEqual(889, size.X, "Adjusted width was incorrect.");
        Assert.AreEqual(shortParent.Y, size.Y, "Adjusted height was incorrect.");
    }

    [TestMethod]
    public void FitWidthAroundBoundsTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Create a parent that is wider than the main size.
        Point wideParent = new(1500, 1000);

        // Fit the size into the parent size.
        Assert.IsTrue(AspectRatioHelpers.FitInBounds(ref size, wideParent, ratio, 2f), "Could not fit into parent bounds.");

        // Ensure the size is correct.
        Assert.AreEqual(wideParent.X, size.X, "Adjusted width was incorrect.");
        Assert.AreEqual(844, size.Y, "Adjusted height was incorrect.");
    }

    [TestMethod]
    public void FitHeightAroundBoundsTest()
    {
        // Create a basic 1000x1000 square.
        Point size = new(1000, 1000);

        // Create a parent that is taller than the main size.
        Point tallParent = new(1000, 1500);

        // Fit the size around the parent size.
        Assert.IsTrue(AspectRatioHelpers.FitAroundBounds(ref size, tallParent, ratio, 2f), "Could not fit around parent bounds.");

        // Ensure the size is correct.
        Assert.AreEqual(2667, size.X, "Adjusted width was incorrect.");
        Assert.AreEqual(tallParent.Y, size.Y, "Adjusted height was incorrect.");
    }
}

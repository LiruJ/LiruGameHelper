using LiruGameHelper.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Numerics;

namespace Tests.Parsers;

[TestClass]
public class VectorTest
{
    [TestMethod]
    public void OneValueTryValidVector3Test()
    {
        Vector3 vector = new(157);
        Assert.IsTrue(ToVector.TryParse("157", out Vector3 output), "TryParse returned false.");
        Assert.AreEqual(vector, output, $"TryParse returned true but the expected vector ({vector}) did not match the returned vector ({output}).");
    }

    [TestMethod]
    public void OneValueValidVector3Test()
    {
        Vector3 vector = new(742);
        Vector3 output = ToVector.Parse3("742");
        Assert.AreEqual(vector, output, $"The expected vector ({vector}) did not match the returned vector ({output}).");
    }

    [TestMethod]
    public void ThreeValueTryValidVector3Test()
    {
        Vector3 vector = new(15, 62, -30);
        Assert.IsTrue(ToVector.TryParse("15, 62, -30", out Vector3 output), "TryParse returned false.");
        Assert.AreEqual(vector, output, $"TryParse returned true but the expected vector ({vector}) did not match the returned vector ({output}).");
    }

    [TestMethod]
    public void ThreeValueValidVector3Test()
    {
        Vector3 vector = new(74, 102, -50);
        Vector3 output = ToVector.Parse3("74, 102, -50");
        Assert.AreEqual(vector, output, $"The expected vector ({vector}) did not match the returned vector ({output}).");
    }

    [TestMethod]
    public void ThreeValueDecimalTest()
    {
        // Save the current culture.
        CultureInfo currentCulture = CultureInfo.CurrentCulture;

        // Set the culture to German, as it uses the ',' character for decimal places which should break parsing if not accounted for.
        CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("de-DE");

        Vector3 vector = new(74.67f, 102.12f, -50.0582f);
        Vector3 output = ToVector.Parse3("74.67, 102.12, -50.0582");

        // Set the culture back so that the error message displays correctly.
        CultureInfo.CurrentCulture = currentCulture;

        Assert.AreEqual(vector, output);
    }
}

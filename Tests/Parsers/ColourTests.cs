using LiruGameHelper.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Reflection;

namespace Tests.Parsers
{
    [TestClass()]
    public class ColourTests
    {
        [TestMethod()]
        public void TryParseTest()
        {
            Assert.IsTrue(Colour.TryParse("#ff0000", out Color colour) && colour == Color.FromArgb(Color.Red.ToArgb()));
            Assert.IsTrue(Colour.TryParse("#f00", out colour) && colour == Color.FromArgb(Color.Red.ToArgb()));
            Assert.IsTrue(Colour.TryParse("#00ff00", out colour) && colour == Color.FromArgb(255, 0, 255, 0));
            Assert.IsTrue(Colour.TryParse("#0f0", out colour) && colour == Color.FromArgb(255, 0, 255, 0));
            Assert.IsTrue(Colour.TryParse("#0000ff", out colour) && colour == Color.FromArgb(Color.Blue.ToArgb()));
            Assert.IsTrue(Colour.TryParse("#00f", out colour) && colour == Color.FromArgb(Color.Blue.ToArgb()));

            foreach (PropertyInfo propertyInfo in typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static))
                Assert.IsTrue(Colour.TryParse(propertyInfo.Name, out colour) && colour == (Color)propertyInfo.GetValue(null));
        }
    }
}
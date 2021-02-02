using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace LiruGameHelperMonoGame.Parsers.Tests
{
    [TestClass()]
    public class ColourTests
    {
        [TestMethod()]
        public void TryParseTest()
        {
            Assert.IsTrue(Colour.TryParse("#ff0000", out Color colour) && colour == Color.Red);
            Assert.IsTrue(Colour.TryParse("#f00", out colour) && colour == Color.Red);
            Assert.IsTrue(Colour.TryParse("#00ff00", out colour) && colour == new Color(0f, 1, 0, 1));
            Assert.IsTrue(Colour.TryParse("#0f0", out colour) && colour == new Color(0f, 1, 0, 1));
            Assert.IsTrue(Colour.TryParse("#0000ff", out colour) && colour == Color.Blue);
            Assert.IsTrue(Colour.TryParse("#00f", out colour) && colour == Color.Blue);

            foreach (PropertyInfo propertyInfo in typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static))
                Assert.IsTrue(Colour.TryParse(propertyInfo.Name, out colour) && colour == (Color)propertyInfo.GetValue(null));
        }
    }
}
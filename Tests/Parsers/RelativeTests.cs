using LiruGameHelper.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Parsers
{
    [TestClass()]
    public class RelativeTests
    {
        [TestMethod()]
        public void TryParseTest()
        {
            Assert.IsTrue(Relative.TryParse("50%", out float value, out bool relative));
            Assert.IsTrue(relative);
            Assert.AreEqual(0.5f, value);
        }
    }
}
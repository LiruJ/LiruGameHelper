using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiruGameHelperMonoGame.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiruGameHelperMonoGame.Parsers.Tests
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
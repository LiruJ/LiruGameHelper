using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiruGameHelperMonoGame.Parsers.Tests
{
    [TestClass]
    public class VectorTest
    {
        [TestMethod]
        public void OneValueTryValidVector3Test()
        {
            Vector3 vector = new Vector3(157);
            Assert.IsTrue(ToVector3.TryParse("157", out Vector3 output), "TryParse returned false.");
            Assert.AreEqual(vector, output, $"TryParse returned true but the expected vector ({vector}) did not match the returned vector ({output}).");
        }

        [TestMethod]
        public void OneValueValidVector3Test()
        {
            Vector3 vector = new Vector3(742);
            Vector3 output = ToVector3.Parse("742");
            Assert.AreEqual(vector, output, $"The expected vector ({vector}) did not match the returned vector ({output}).");
        }

        [TestMethod]
        public void ThreeValueTryValidVector3Test()
        {
            Vector3 vector = new Vector3(15, 62, -30);
            Assert.IsTrue(ToVector3.TryParse("15, 62, -30", out Vector3 output), "TryParse returned false.");
            Assert.AreEqual(vector, output, $"TryParse returned true but the expected vector ({vector}) did not match the returned vector ({output}).");
        }

        [TestMethod]
        public void ThreeValueValidVector3Test()
        {
            Vector3 vector = new Vector3(74, 102, -50);
            Vector3 output = ToVector3.Parse("74, 102, -50");
            Assert.AreEqual(vector, output, $"The expected vector ({vector}) did not match the returned vector ({output}).");
        }
    }
}

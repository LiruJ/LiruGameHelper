using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LiruGameHelper.Signals.Tests
{
    [TestClass()]
    public class SignalTests
    {
        [TestMethod()]
        public void ConnectTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod()]
        public void ConnectOneTimeTest()
        {
            // Create a new signal and connect it once.
            Signal signal = new Signal();
            Signal<int> signal2 = new Signal<int>();
            Signal<int, int> signal3 = new Signal<int, int>();

            SignalConnection connection = signal.ConnectOneTime(oneTime);
            signal2.ConnectOneTime((int t) => oneTime());
            signal3.ConnectOneTime((int t, int f) => oneTime());

            // Invoke the signal twice.
            signal.Invoke();
            signal.Invoke();

            // Try to disconnect the connection.
            Assert.ThrowsException<Exceptions.InvalidDisconnectionException>(() => connection.Disconnect());

            signal2.Invoke(0);
            signal2.Invoke(0);

            signal3.Invoke(0, 0);
            signal3.Invoke(0, 0);

            // Assert that the connected function only ran once for each signal.
            Assert.AreEqual(3, i);
        }

        [TestMethod()]
        public void DisconnectTest()
        {
            Assert.Inconclusive();

        }

        [TestMethod()]
        public void InvokeTest()
        {
            Assert.Inconclusive();

        }

        [TestMethod]
        public void RecursiveTest()
        {
            Signal signal = new Signal();

            recursive(signal, 3);

            for (int i = 0; i < 3; i++)
            {
                signal.Invoke();
            }

            Assert.AreEqual(3, r);
        }

        int r = 0;
        private void recursive(Signal signal, int count)
        {
            r++;

            if (count > 1) signal.ConnectOneTime(() => recursive(signal, count - 1));
        }

        int i = 0;
        private void oneTime()
        {
            i++;
        }
    }
}
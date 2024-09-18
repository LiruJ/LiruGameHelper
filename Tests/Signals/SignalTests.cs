using LiruGameHelper.Signals;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Signals
{
    [TestClass()]
    public class SignalTests
    {
        [TestMethod]
        public void ConnectTest()
        {
            Signal signal1 = new Signal();
            Signal<int> signal2 = new Signal<int>();
            Signal<int, int> signal3 = new Signal<int, int>();

            SignalConnection connection1 = signal1.Connect(() => { });
            SignalConnection connection2 = signal2.Connect((i) => { });
            SignalConnection connection3 = signal3.Connect((i, j) => { });

            Assert.AreEqual(1, signal1.BindingsCount);
            Assert.AreEqual(1, signal2.BindingsCount);
            Assert.AreEqual(1, signal3.BindingsCount);
        }

        [TestMethod()]
        public void ConnectOneTimeTest()
        {
            // Create a new signal and connect it once.
            Signal signal = new Signal();
            Signal<int> signal2 = new Signal<int>();
            Signal<int, int> signal3 = new Signal<int, int>();

            SignalConnection connection = signal.ConnectOneTime(oneTime);
            signal2.ConnectOneTime((t) => oneTime());
            signal3.ConnectOneTime((t, f) => oneTime());

            // Invoke the signal twice.
            signal.Invoke();
            signal.Invoke();

            // Try to disconnect the connection.
#if DEBUG
            Assert.ThrowsException<Exceptions.InvalidDisconnectionException>(() => connection.Disconnect());
#endif

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
            Signal signal1 = new Signal();
            Signal<int> signal2 = new Signal<int>();
            Signal<int, int> signal3 = new Signal<int, int>();

            bool invoked1 = false, invoked2 = false, invoked3 = false;
            SignalConnection connection1 = signal1.Connect(() => invoked1 = true);
            SignalConnection connection2 = signal2.Connect((i) => invoked2 = true);
            SignalConnection connection3 = signal3.Connect((i, j) => invoked3 = true);

            connection1.Disconnect();
            connection2.Disconnect();
            connection3.Disconnect();

            signal1.Invoke();
            signal2.Invoke(0);
            signal3.Invoke(0, 0);

            Assert.IsFalse(invoked1);
            Assert.IsFalse(invoked2);
            Assert.IsFalse(invoked3);
            Assert.AreEqual(0, signal1.BindingsCount);
            Assert.AreEqual(0, signal2.BindingsCount);
            Assert.AreEqual(0, signal3.BindingsCount);
        }

        [TestMethod]
        public void DisconnectAllTest()
        {
            // Create a new signal.
            Signal signal = new Signal();

            int bindingAmount = 15;

            for (int i = 0; i < bindingAmount; i++)
                signal.Connect(oneTime);

            Assert.AreEqual(bindingAmount, signal.BindingsCount, "Invalid binding amount.");
            signal.DisconnectAll();
            Assert.AreEqual(0, signal.BindingsCount);
        }

        [TestMethod()]
        public void InvokeTest()
        {
            Signal signal1 = new Signal();
            Signal<int> signal2 = new Signal<int>();
            Signal<int, int> signal3 = new Signal<int, int>();

            bool invoked1 = false, invoked2 = false, invoked3 = false;

            signal1.Connect(() => invoked1 = true);
            signal2.Connect((i) => invoked2 = true);
            signal3.Connect((i, j) => invoked3 = true);

            signal1.Invoke();
            signal2.Invoke(0);
            signal3.Invoke(0, 0);

            Assert.IsTrue(invoked1);
            Assert.IsTrue(invoked2);
            Assert.IsTrue(invoked3);
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
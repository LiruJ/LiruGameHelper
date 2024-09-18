using LiruGameHelper.Exceptions;
using System;
using System.Collections.Generic;

namespace LiruGameHelper.Signals
{
    /// <summary> Allows for SignalConnections to disconnect without having access to any other functions. </summary>
    internal interface ISignal
    {
        void Disconnect(SignalConnection connection);
    }

    internal struct Binding
    {
        #region Properties
        /// <summary> <c>true</c> if the bound event should only fire once. </summary>
        public bool OneTime { get; }

        /// <summary> The ID of the binding. </summary>
        public uint ID { get; }

        /// <summary> The action bound to the ID. </summary>
        public Action Action { get; }
        #endregion

        #region Constructors
        public Binding(uint id, Action action, bool oneTime)
        {
            ID = id;
            Action = action;
            OneTime = oneTime;
        }
        #endregion
    }

    internal struct Binding<T1>
    {
        #region Properties
        /// <summary> <c>true</c> if the bound event should only fire once. </summary>
        public bool OneTime { get; set; }

        /// <summary> The ID of the binding, represents the index within the signal. </summary>
        public uint ID { get; set; }

        /// <summary> The action bound to the ID. </summary>
        public Action<T1> Action { get; set; }
        #endregion

        #region Constructors
        public Binding(uint id, Action<T1> action, bool oneTime)
        {
            ID = id;
            Action = action;
            OneTime = oneTime;
        }
        #endregion
    }

    internal struct Binding<T1, T2>
    {
        #region Properties
        /// <summary> <c>true</c> if the bound event should only fire once. </summary>
        public bool OneTime { get; set; }

        /// <summary> The ID of the binding. </summary>
        public uint ID { get; set; }

        /// <summary> The action bound to the ID. </summary>
        public Action<T1, T2> Action { get; set; }
        #endregion

        #region Constructors
        public Binding(uint id, Action<T1, T2> action, bool oneTime)
        {
            ID = id;
            Action = action;
            OneTime = oneTime;
        }
        #endregion
    }

    /// <summary> Represents a Signal with no return value and no parameters. </summary>
    public class Signal : ISignal, IConnectableSignal
    {
        #region Binding Fields
        /// <summary> The collection of functions that are bound to this signal. </summary>
        private readonly Dictionary<uint, Binding> bindings = new Dictionary<uint, Binding>();

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding> bindingsCopy = new List<Binding>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private uint totalBindings = 0;
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Count;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection ConnectOneTime(Action action) => connect(action, true);

        /// <summary> Connects the given <paramref name="action"/> with the given <paramref name="oneTime"/> flag. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <param name="oneTime"> <c>true</c> if the function should only be called once; otherwise <c>false</c>. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        private SignalConnection connect(Action action, bool oneTime)
        {
            // Add the new binding to the array
            bindings.Add(totalBindings, new Binding(totalBindings, action, oneTime));

            // As a new binding was just added, increment the total and current amount of bindings
            totalBindings++;

            // Return a new connection.
            return new SignalConnection(totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            // Check that the connection's signal is this signal, otherwise throw an exception.
            if (connection.ConnectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            // Get the binding that the connection represents.
            if (!bindings.TryGetValue(connection.ID, out Binding binding)) throw new InvalidDisconnectionException("Connection ID does not exist.");

            // Disconnect the binding.
            disconnect(binding.ID);
        }

        /// <summary> Disconnects all bound functions from this signal. </summary>
        public void DisconnectAll() => bindings.Clear();

        /// <summary> Disconnects the binding with the given <paramref name="bindingID"/> from this signal. </summary>
        /// <param name="bindingID"> The ID of the binding to remove. </param>
        private void disconnect(uint bindingID) => bindings.Remove(bindingID);
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions. </summary>
        public void Invoke()
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            foreach (Binding binding in bindings.Values)
                bindingsCopy.Add(binding);

            // Invoke each action.
            foreach (Binding binding in bindingsCopy)
            {
                // Invoke the action.
                binding.Action.Invoke();

                // If the binding was one time, disonnect it.
                if (binding.OneTime) disconnect(binding.ID);
            }
        }
        #endregion
    }

    public class Signal<T1> : ISignal, IConnectableSignal<T1>
    {
        #region Binding Fields
        /// <summary> The collection of functions that are bound to this signal. </summary>
        private readonly Dictionary<uint, Binding<T1>> bindings = new Dictionary<uint, Binding<T1>>();

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding<T1>> bindingsCopy = new List<Binding<T1>>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private uint totalBindings = 0;
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Count;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action<T1> action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection ConnectOneTime(Action<T1> action) => connect(action, true);

        /// <summary> Connects the given <paramref name="action"/> with the given <paramref name="oneTime"/> flag. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <param name="oneTime"> <c>true</c> if the function should only be called once; otherwise <c>false</c>. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        private SignalConnection connect(Action<T1> action, bool oneTime)
        {
            // Add the new binding to the array
            bindings.Add(totalBindings, new Binding<T1>(totalBindings, action, oneTime));

            // As a new binding was just added, increment the total and current amount of bindings
            totalBindings++;

            // Return a new connection.
            return new SignalConnection(totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            // Check that the connection's signal is this signal, otherwise throw an exception.
            if (connection.ConnectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            // Get the binding that the connection represents.
            if (!bindings.TryGetValue(connection.ID, out Binding<T1> binding)) throw new InvalidDisconnectionException("Connection ID does not exist.");

            // Disconnect the binding.
            disconnect(binding.ID);
        }

        /// <summary> Disconnects all bound functions from this signal. </summary>
        public void DisconnectAll() => bindings.Clear();

        /// <summary> Disconnects the binding with the given <paramref name="bindingID"/> from this signal. </summary>
        /// <param name="bindingID"> The ID of the binding to remove. </param>
        private void disconnect(uint bindingID) => bindings.Remove(bindingID);
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions with the given argument. </summary>
        /// <param name="input"> The given argument. </param>
        public void Invoke(T1 input)
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            foreach (Binding<T1> binding in bindings.Values)
                bindingsCopy.Add(binding);

            // Invoke each action.
            foreach (Binding<T1> binding in bindingsCopy)
            {
                // Invoke the action.
                binding.Action.Invoke(input);

                // If the binding was one time, disonnect it.
                if (binding.OneTime) disconnect(binding.ID);
            }
        }
        #endregion
    }

    public class Signal<T1, T2> : ISignal, IConnectableSignal<T1, T2>
    {
        #region Binding Fields
        /// <summary> The collection of functions that are bound to this signal. </summary>
        private readonly Dictionary<uint, Binding<T1, T2>> bindings = new Dictionary<uint, Binding<T1, T2>>();

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding<T1, T2>> bindingsCopy = new List<Binding<T1, T2>>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private uint totalBindings = 0;
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Count;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action<T1, T2> action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection ConnectOneTime(Action<T1, T2> action) => connect(action, true);

        /// <summary> Connects the given <paramref name="action"/> with the given <paramref name="oneTime"/> flag. </summary>
        /// <param name="action"> The function that is invoked. </param>
        /// <param name="oneTime"> <c>true</c> if the function should only be called once; otherwise <c>false</c>. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        private SignalConnection connect(Action<T1, T2> action, bool oneTime)
        {
            // Add the new binding to the array
            bindings.Add(totalBindings, new Binding<T1, T2>(totalBindings, action, oneTime));

            // As a new binding was just added, increment the total and current amount of bindings
            totalBindings++;

            // Return a new connection.
            return new SignalConnection(totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            // Check that the connection's signal is this signal, otherwise throw an exception.
            if (connection.ConnectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            // Get the binding that the connection represents.
            if (!bindings.TryGetValue(connection.ID, out Binding<T1, T2> binding)) throw new InvalidDisconnectionException("Connection ID does not exist.");

            // Disconnect the binding.
            disconnect(binding.ID);
        }

        /// <summary> Disconnects all bound functions from this signal. </summary>
        public void DisconnectAll() => bindings.Clear();

        /// <summary> Disconnects the binding with the given <paramref name="bindingID"/> from this signal. </summary>
        /// <param name="bindingID"> The ID of the binding to remove. </param>
        private void disconnect(uint bindingID) => bindings.Remove(bindingID);
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions with the given arguments. </summary>
        /// <param name="input"> The given argument. </param>
        /// <param name="input"> The second given argument. </param>
        public void Invoke(T1 firstInput, T2 secondInput)
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            foreach (Binding<T1, T2> binding in bindings.Values)
                bindingsCopy.Add(binding);

            // Invoke each action.
            foreach (Binding<T1, T2> binding in bindingsCopy)
            {
                // Invoke the action.
                binding.Action.Invoke(firstInput, secondInput);

                // If the binding was one time, disonnect it.
                if (binding.OneTime) disconnect(binding.ID);
            }
        }
        #endregion
    }
}

using LiruGameHelper.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public int ID { get; }

        /// <summary> The action bound to the ID. </summary>
        public Action Action { get; }
        #endregion

        #region Constructors
        public Binding(int id, Action action, bool oneTime)
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
        public int ID { get; set; }

        /// <summary> The action bound to the ID. </summary>
        public Action<T1> Action { get; set; }
        #endregion

        #region Constructors
        public Binding(int id, Action<T1> action, bool oneTime)
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
        public int ID { get; set; }

        /// <summary> The action bound to the ID. </summary>
        public Action<T1, T2> Action { get; set; }
        #endregion

        #region Constructors
        public Binding(int id, Action<T1, T2> action, bool oneTime)
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
        #region Constant Fields
        /// <summary> Used to represent an array position with no contents. </summary>
        internal const int EmptyIndex = -1;
        #endregion

        #region Binding Fields
        /// <summary> The array of functions that are bound to this signal. </summary>
        private Binding[] bindings = new Binding[0];

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding> bindingsCopy = new List<Binding>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private int totalBindings = 0;

        /// <summary> The current amount of functions bound to this signal. </summary>
        private int currentBindings = 0;

        /// <summary> Tracks which indices of the bindings array are empty, so they can be reused. </summary>
        private readonly Queue<int> emptyIndices = new Queue<int>();
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Length;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function to add. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SignalConnection ConnectOneTime(Action action) => connect(action, true);

        private SignalConnection connect(Action action, bool oneTime)
        {
            //By default, attempt to append the new binding on the end of the bindings array
            int newConnectionIndex = currentBindings;

            //If there is an unused space in the queue, reuse it
            if (emptyIndices.Count > 0) newConnectionIndex = emptyIndices.Dequeue();

            //Otherwise if the array of bindings needs to resize to fit the new addition, do so, and set the new item to be empty
            else if (currentBindings >= bindings.Length)
            {
                Array.Resize(ref bindings, currentBindings + 1);
                bindingsCopy.Capacity = currentBindings + 1;
                bindings[currentBindings] = new Binding(EmptyIndex, null, false);
            }

            //If there is already a binding at the current ID, throw an error
            if (bindings[newConnectionIndex].ID != EmptyIndex) throw new InvalidOperationException("Attempted to overwrite an existing bind.");

            //Add the new binding to the array
            bindings[newConnectionIndex] = new Binding(newConnectionIndex, action, oneTime);

            //As we've just added a new binding, increment the total and current amount of bindings
            totalBindings++;
            currentBindings++;

            //Return a new connection
            return new SignalConnection(newConnectionIndex, totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            //Check that the connection's signal is this signal, otherwise throw an exception
            if (connection.connectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            //Get the binding that the connection represents
            Binding binding = bindings[connection.ID];

            // If the connection is empty, or the IDs do not match, do nothing.
            if (binding.Action == null || binding.ID == EmptyIndex || binding.ID != connection.BirthID)
#if DEBUG
                throw new InvalidDisconnectionException("Cannot disconnect the same connection twice.");
#else
                return;
#endif
            // Disconnect the binding.
            disconnect(binding.ID);
        }

        private void disconnect(int bindingID)
        {
            //Set the binding to null
            bindings[bindingID] = new Binding(EmptyIndex, null, false);

            //Add the now empty index to the unused indices, so it may be reused
            emptyIndices.Enqueue(bindingID);

            //Decrease the current amount of bindings
            currentBindings--;
        }
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions. </summary>
        public void Invoke()
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            for (int i = 0; i < BindingsCount; i++)
                bindingsCopy.Add(bindings[i]);

            // Loops over every bound function, checks that it's valid, and calls it.
            for (int i = 0; i < bindingsCopy.Count; i++)
            {
                // If the binding is not empty.
                if (bindingsCopy[i].ID != EmptyIndex)
                {
                    // Call the function.
                    bindingsCopy[i].Action.Invoke();

                    // If the binding was one time, disconnect it.
                    if (bindingsCopy[i].OneTime) disconnect(i);
                }
            }
        }
        #endregion
    }

    public class Signal<T1> : ISignal, IConnectableSignal<T1>
    {
        #region Binding Fields
        /// <summary> The array of functions that are bound to this signal. </summary>
        private Binding<T1>[] bindings = new Binding<T1>[0];

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding<T1>> bindingsCopy = new List<Binding<T1>>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private int totalBindings = 0;

        /// <summary> The current amount of functions bound to this signal. </summary>
        private int currentBindings = 0;

        /// <summary> Tracks which indices of the bindings array are empty, so they can be reused. </summary>
        private readonly Queue<int> emptyIndices = new Queue<int>();
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Length;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function to add. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action<T1> action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal{T1}"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SignalConnection ConnectOneTime(Action<T1> action) => connect(action, true);

        private SignalConnection connect(Action<T1> action, bool oneTime)
        {
            //By default, attempt to append the new binding on the end of the bindings array
            int newConnectionIndex = currentBindings;

            //If there is an unused space in the queue, reuse it
            if (emptyIndices.Count > 0) newConnectionIndex = emptyIndices.Dequeue();

            //Otherwise if the array of bindings needs to resize to fit the new addition, do so, and set the new item to be empty
            else if (currentBindings >= bindings.Length)
            {
                Array.Resize(ref bindings, currentBindings + 1);
                bindingsCopy.Capacity = currentBindings + 1;
                bindings[currentBindings] = new Binding<T1>(Signal.EmptyIndex, null, false);
            }

            //If there is already a binding at the current ID, throw an error
            if (bindings[newConnectionIndex].ID != Signal.EmptyIndex) throw new InvalidOperationException("Attempted to overwrite an existing bind.");

            //Add the new binding to the array
            bindings[newConnectionIndex] = new Binding<T1>(newConnectionIndex, action, oneTime);

            //As we've just added a new binding, increment the total and current amount of bindings
            totalBindings++;
            currentBindings++;

            //Return a new connection
            return new SignalConnection(newConnectionIndex, totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            //Check that the connection's signal is this signal, otherwise throw an exception
            if (connection.connectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            //Get the binding that the connection represents
            Binding<T1> binding = bindings[connection.ID];

            //If the connection is empty, or the IDs do not match, throw an exception
            if (binding.Action == null || binding.ID == Signal.EmptyIndex || binding.ID != connection.BirthID)
#if DEBUG
                throw new InvalidDisconnectionException("Cannot disconnect the same connection twice.");
#else
                return;
#endif

            // Disconnect the binding.
            disconnect(binding);
        }

        private void disconnect(Binding<T1> binding)
        {
            //Set the binding to null
            bindings[binding.ID] = new Binding<T1>(Signal.EmptyIndex, null, false);

            //Add the now empty index to the unused indices, so it may be reused
            emptyIndices.Enqueue(binding.ID);

            //Decrease the current amount of bindings
            currentBindings--;
        }
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions with the given argument. </summary>
        /// <param name="input"> The given argument. </param>
        public void Invoke(T1 input)
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            for (int i = 0; i < BindingsCount; i++)
                bindingsCopy.Add(bindings[i]);

            // Loops over every bound function, checks that it's valid, and calls it.
            for (int i = 0; i < bindingsCopy.Count; i++)
            {
                // Call the function.
                if (bindingsCopy[i].ID != Signal.EmptyIndex) bindingsCopy[i].Action.Invoke(input);

                // If the binding was one time, disconnect it.
                if (bindingsCopy[i].OneTime) disconnect(bindingsCopy[i]);
            }
        }
        #endregion
    }

    public class Signal<T1, T2> : ISignal, IConnectableSignal<T1, T2>
    {
        #region Binding Fields
        /// <summary> The array of functions that are bound to this signal. </summary>
        private Binding<T1, T2>[] bindings = new Binding<T1, T2>[0];

        /// <summary> A collection of bindings that is populated from the main collection every time <see cref="Invoke"/> is called. </summary>
        private readonly List<Binding<T1, T2>> bindingsCopy = new List<Binding<T1, T2>>();

        /// <summary> The total amount of functions that have been bound to this signal, including the ones that were removed. </summary>
        private int totalBindings = 0;

        /// <summary> The current amount of functions bound to this signal. </summary>
        private int currentBindings = 0;

        /// <summary> Tracks which indices of the bindings array are empty, so they can be reused. </summary>
        private readonly Queue<int> emptyIndices = new Queue<int>();
        #endregion

        #region Properties
        /// <summary> The total number of bindings this signal has. </summary>
        public int BindingsCount => bindings.Length;
        #endregion

        #region Connection Functions
        /// <summary> Connect the given function to this signal, calling it when the signal is invoked. </summary>
        /// <param name="action"> The function to add. </param>
        /// <returns> A <see cref="SignalConnection"/> for the created binding. </returns>
        public SignalConnection Connect(Action<T1, T2> action) => connect(action, false);

        /// <summary> Connect the given function to this <see cref="Signal{T1}"/>, calling it when invoked then immediately disconnecting. </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public SignalConnection ConnectOneTime(Action<T1, T2> action) => connect(action, true);

        private SignalConnection connect(Action<T1, T2> action, bool oneTime)
        {
            //By default, attempt to append the new binding on the end of the bindings array
            int newConnectionIndex = currentBindings;

            //If there is an unused space in the queue, reuse it
            if (emptyIndices.Count > 0) newConnectionIndex = emptyIndices.Dequeue();

            //Otherwise if the array of bindings needs to resize to fit the new addition, do so, and set the new item to be empty
            else if (currentBindings >= bindings.Length)
            {
                Array.Resize(ref bindings, currentBindings + 1);
                bindingsCopy.Capacity = currentBindings + 1;
                bindings[currentBindings] = new Binding<T1, T2>(Signal.EmptyIndex, null, false);
            }

            //If there is already a binding at the current ID, throw an error
            if (bindings[newConnectionIndex].ID != Signal.EmptyIndex) throw new InvalidOperationException("Attempted to overwrite an existing bind.");

            //Add the new binding to the array
            bindings[newConnectionIndex] = new Binding<T1, T2>(newConnectionIndex, action, oneTime);

            //As we've just added a new binding, increment the total and current amount of bindings
            totalBindings++;
            currentBindings++;

            //Return a new connection
            return new SignalConnection(newConnectionIndex, totalBindings - 1, this);
        }

        /// <summary> Disconnects the given connection. connection.Disconnect() is also valid. </summary>
        /// <param name="connection"> The connection to disconnect. </param>
        public void Disconnect(SignalConnection connection)
        {
            //Check that the connection's signal is this signal, otherwise throw an exception
            if (connection.connectedSignal != this) throw new InvalidDisconnectionException("SignalConnection given to Signal does not belong to this Signal.");

            //Get the binding that the connection represents
            Binding<T1, T2> binding = bindings[connection.ID];

            //If the connection is empty, or the IDs do not match, throw an exception
            if (binding.Action == null || binding.ID == Signal.EmptyIndex || binding.ID != connection.BirthID)
#if DEBUG
                throw new InvalidDisconnectionException("Cannot disconnect the same connection twice.");
#else
                return;
#endif

            // Disconnect the binding.
            disconnect(binding);
        }

        private void disconnect(Binding<T1, T2> binding)
        {
            //Set the binding to null
            bindings[binding.ID] = new Binding<T1, T2>(Signal.EmptyIndex, null, false);

            //Add the now empty index to the unused indices, so it may be reused
            emptyIndices.Enqueue(binding.ID);

            //Decrease the current amount of bindings
            currentBindings--;
        }
        #endregion

        #region Invocation Functions
        /// <summary> Calls all of the bound functions with the given argument. </summary>
        /// <param name="input"> The given argument. </param>
        public void Invoke(T1 firstInput, T2 secondInput)
        {
            // Copy the bindings, so that the signal can be worked on from any called function.
            bindingsCopy.Clear();
            for (int i = 0; i < BindingsCount; i++)
                bindingsCopy.Add(bindings[i]);

            // Loops over every bound function, checks that it's valid, and calls it.
            for (int i = 0; i < bindingsCopy.Count; i++)
            {
                // Call the function.
                if (bindingsCopy[i].ID != Signal.EmptyIndex) bindingsCopy[i].Action.Invoke(firstInput, secondInput);

                // If the binding was one time, disconnect it.
                if (bindingsCopy[i].OneTime) disconnect(bindingsCopy[i]);
            }
        }
        #endregion
    }
}

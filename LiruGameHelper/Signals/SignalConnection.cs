using System;

namespace LiruGameHelper.Signals
{
    public struct SignalConnection
    {
        #region Properties
        /// <summary> The ID of this connection, represents the index of a binding. </summary>
        internal uint ID { get; }

        /// <summary> The signal that contains the binding of this connection. </summary>
        internal ISignal ConnectedSignal { get; }
        #endregion

        #region Constructors
        /// <summary> Creates a new SignalConnection with the given properties. </summary>
        /// <param name="id"> The ID of the binding. </param>
        /// <param name="signal"> The signal that contains the binding. </param>
        internal SignalConnection(uint id, ISignal signal)
        {
            //Set the properties
            ID = id;

            //If the given signal is null, throw an error, otherwise set the Signal property
            ConnectedSignal = signal ?? throw new ArgumentNullException("Given Signal cannot be null.");
        }
        #endregion

        #region Connection Functions
        /// <summary> Disconnects the binding from the underlying signal. </summary>
        public void Disconnect()
        {
            // If the connected signal is null, it is probably an empty struct, so do nothing.
            if (ConnectedSignal == null) return;

            // Disconnect the signal
            ConnectedSignal.Disconnect(this);
        }
        #endregion
    }
}

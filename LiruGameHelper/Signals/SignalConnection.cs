using System;

namespace LiruGameHelper.Signals
{
    public struct SignalConnection
    {
        #region ID Properties
        /// <summary> The ID of this connection, represents the index of a binding. </summary>
        internal int ID { get; private set; }

        /// <summary> The unique BirthID of this connection. </summary>
        internal int BirthID { get; private set; }
        #endregion

        #region SIGNAL PROPERTIES
        /// <summary> The signal that contains the binding of this connection. </summary>
        internal ISignal connectedSignal;
        #endregion

        #region Connection Functions
        /// <summary> Disconnects the binding from the underlying signal. </summary>
        public void Disconnect()
        {
            // If the connected signal is null, it is probably an empty struct, so do nothing.
            if (connectedSignal == null) return;

            // Disconnect the signal
            connectedSignal.Disconnect(this);
        }
        #endregion

        #region Constructors
        /// <summary> Creates a new SignalConnection with the given properties. </summary>
        /// <param name="id"> The ID of the binding. </param>
        /// <param name="birthID"> The BirthID of the binding. </param>
        /// <param name="signal"> The signal that contains the binding. </param>
        internal SignalConnection(int id, int birthID, ISignal signal)
        {
            //If the ID or BirthID are invalid, throw an exception
            if (id < 0) throw new ArgumentException("ID for a SignalConnection cannot be negative.");
            if (birthID < 0) throw new ArgumentException("BirthID for a SignalConnection cannot be negative.");

            //Set the properties
            ID = id;
            BirthID = birthID;

            //If the given signal is null, throw an error, otherwise set the Signal property
            connectedSignal = signal ?? throw new ArgumentNullException("Given Signal cannot be null.");
        }
        #endregion
    }
}

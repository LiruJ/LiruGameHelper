namespace LiruGameHelper.Signals
{
    /// <summary> Represents a read-only version of a <see cref="SignalProperty{T1}"/>, </summary>
    /// <typeparam name="T1"></typeparam>
    public interface IReadOnlySignalProperty<T1>
    {
        /// <summary> Gets the underlying value. </summary>
        T1 Property { get; }

        /// <summary> Fires when the underlying value is changed. </summary>
        IConnectableSignal<T1> OnChanged { get; }
    }

    /// <summary> Represents a property with a <see cref="Signal"/> that fires on change. </summary>
    public struct SignalProperty<T1>
    {
        #region Fields
        /// <summary> The value field. </summary>
        private T1 property;

        /// <summary> The signal that fires when the value is changed. </summary>
        private readonly Signal<T1> onChanged;
        #endregion

        #region Properties
        /// <summary> Gets or sets the underlying value. </summary>
        public T1 Property
        {
            get => property;
            set
            {
                // If the value will not change, do nothing.
                if (value.Equals(property)) return;

                // Set the value.
                property = value;

                // Fire the signal.
                onChanged.Invoke(property);
            }
        }

        /// <summary> Fires when the underlying value is changed. </summary>
        public IConnectableSignal<T1> OnChanged { get => onChanged; }
        #endregion

        #region Constructors
        /// <summary> Creates a new <see cref="SignalProperty{T1}"/> with the default value for the given type. </summary>


        /// <summary> Creates a new <see cref="SignalProperty{T1}"/> with the given value. </summary>
        /// <param name="value"> The initial value. </param>
        public SignalProperty(T1 value)
        {
            property = value;
            onChanged = new Signal<T1>();
        }
        #endregion

        #region Operator Functions
        /// <summary> Implicit conversion from <see cref="SignalProperty{T1}"/> to <typeparamref name="T1"/>. </summary>
        public static implicit operator T1(SignalProperty<T1> signalProperty) => signalProperty.property;
        #endregion
    }
}

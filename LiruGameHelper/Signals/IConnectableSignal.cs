using System;

namespace LiruGameHelper.Signals;

public interface IConnectableSignal
{
    #region Connection Functions
    SignalConnection Connect(Action action);

    SignalConnection ConnectOneTime(Action action);
    #endregion
}

public interface IConnectableSignal<T1>
{
    #region Connection Functions
    SignalConnection Connect(Action<T1> action);

    SignalConnection ConnectOneTime(Action<T1> action);
    #endregion
}

public interface IConnectableSignal<T1, T2>
{
    #region Connection Functions
    SignalConnection Connect(Action<T1, T2> action);

    SignalConnection ConnectOneTime(Action<T1, T2> action);
    #endregion
}

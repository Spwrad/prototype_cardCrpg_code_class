using System;

namespace prototype_cardCrpg_code_class.GSignalSystem;

public class GSignalManager
{
    private static readonly System.Collections.Generic.Dictionary<Type, Delegate> signalMap = new();
    
    #region API
    public static void Emit<T>(T _signalData)
    {
        if (signalMap.TryGetValue(typeof(T), out var existingSignalEvent))
        {
            if (existingSignalEvent is Action<T> typedSignalEvent) 
                typedSignalEvent.Invoke(_signalData);
        }
    }

    public static void Register<T>(Action<T> _onSignal)
    {
        if (signalMap.TryGetValue(typeof(T), out var existingSignalEvent))
        {
            signalMap[typeof(T)] = Delegate.Combine(existingSignalEvent, _onSignal);
        }
        else
        {
            signalMap[typeof(T)] = _onSignal;
        }
    }

    public static void Unregister<T>(Action<T> _onSignal)
    {
        if (signalMap.TryGetValue(typeof(T), out var existingCallback))
        {
            var newSignalEvent = Delegate.Remove(existingCallback, _onSignal);
            if (newSignalEvent == null)
                signalMap.Remove(typeof(T));
            else
                signalMap[typeof(T)] = newSignalEvent;
        }
    }
    #endregion
}
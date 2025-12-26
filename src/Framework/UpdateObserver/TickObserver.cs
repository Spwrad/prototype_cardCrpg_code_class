using System;
using System.Collections.Generic;
using Godot;

namespace prototype_cardCrpg_code_class.Framework.UpdateObserver;

file static class TickRegistry
{
    internal static readonly Dictionary<object, Action<double>> TickEvents = new();
    internal static readonly Dictionary<object, Action<double>> FixedTickEvents = new();
}
public partial class TickObserver : Node
{
    public override void _Process(double _delta)
    {
        foreach (var tickEvent in TickRegistry.TickEvents)
        {
            tickEvent.Value(_delta);
        }
    }

    public override void _PhysicsProcess(double _delta)
    {
        foreach (var fixedTickEvent in TickRegistry.FixedTickEvents)
        {
            fixedTickEvent.Value(_delta);
        }
    }
}

public static class TickObserverExtension
{
    
    public static bool RegisterTick(this object _requester, Action<double> _tickEvent)
    {
        if (_tickEvent is null) return false;
        
        TickRegistry.TickEvents[_requester] = _tickEvent;
        return true;
    }

    public static bool RegisterFixedTick(this object _requester, Action<double> _fixedTickEvent)
    {
        if (_fixedTickEvent is null) return false;
        
        TickRegistry.FixedTickEvents[_requester] = _fixedTickEvent;
        return true;
    }

    public static void UnregisterTick(this object _requester)
    {
        TickRegistry.TickEvents.Remove(_requester);
    }
    public static void UnregisterFixedTick(this object _requester)
    {
        TickRegistry.FixedTickEvents.Remove(_requester);
    }
}
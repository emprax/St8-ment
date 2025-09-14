using System;
using System.Collections.Generic;

namespace St8Ment.States.Core;

public class State(StateId id, params Action<StateId, StateId>[] listeners)
{
    private readonly List<Action<StateId, StateId>> listeners = [.. listeners];

    public State(StateId id) : this(id, []) { }

    public StateId Id
    {
        get => id;
        set
        {
            var previous = id;

            id = value;
            this.listeners.ForEach(l => l(previous, id));
        }
    }

    public void RegisterListener(Action<StateId, StateId> listener) => this.listeners.Add(listener);
}
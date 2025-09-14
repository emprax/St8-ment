using St8Ment.StateMachines.Builders;
using St8Ment.StateMachines.Components;
using System;
using System.Collections.Generic;

namespace St8Ment.StateMachines;

public class StateMachineProvider<TKey> : IStateMachineProvider<TKey>
{
    private readonly IDictionary<TKey, Func<IStateMachineCore>> stateMachines;

    public StateMachineProvider(IDictionary<TKey, Func<IStateMachineCore>> stateMachines) => this.stateMachines = stateMachines;

    public IStateMachine? Get(TKey key) => this.stateMachines.TryGetValue(key, out var factory) && factory is not null
        ? new StateMachine(factory.Invoke())
        : null;
}
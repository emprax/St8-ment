using St8Ment.StateMachines.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace St8Ment.StateMachines.Builders;

internal class StateMachineFactoryBuilder<TKey> : IStateMachineFactoryBuilder<TKey> where TKey : notnull
{
    private readonly IDictionary<TKey, Func<IStateMachineCore>> stateMachines;

    internal StateMachineFactoryBuilder() => this.stateMachines = new ConcurrentDictionary<TKey, Func<IStateMachineCore>>();

    public IStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IInitialStateComponentBuilder> configuration)
    {
        this.Remove(key);
        this.stateMachines.Add(key, new Func<IStateMachineCore>(() =>
        { 
            var component = new StateComponentCollection();
            var builder = new InitialStateComponentBuilder(component);
            configuration.Invoke(builder);
            
            return new StateMachineCore(builder.InitialState, component);
        }));

        return this;
    }

    private void Remove(TKey key) => this.stateMachines.Remove(key);

    internal ConcurrentDictionary<TKey, Func<IStateMachineCore>> Build()
    {
        return new ConcurrentDictionary<TKey, Func<IStateMachineCore>>(this.stateMachines.ToDictionary(
            k => k.Key, 
            k => new Func<IStateMachineCore>(() => k.Value.Invoke())));
    }
}
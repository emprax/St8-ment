using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using St8Ment.DependencyInjection.StateMachines.Abstractions;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedStateMachineFactoryBuilder<TKey> : IExtendedStateMachineFactoryBuilder<TKey> where TKey : notnull
{
    private readonly IDictionary<TKey, Func<IServiceProvider, IStateMachineCore>> stateMachines;

    internal ExtendedStateMachineFactoryBuilder() => this.stateMachines = new ConcurrentDictionary<TKey, Func<IServiceProvider, IStateMachineCore>>();

    public IExtendedStateMachineFactoryBuilder<TKey> AddStateMachine(TKey key, Action<IExtendedInitialStateComponentBuilder> configuration)
    {
        this.Remove(key);
        this.stateMachines.Add(key, new Func<IServiceProvider, IStateMachineCore>(provider =>
        { 
            var component = new StateComponentCollection();
            var factory = new DependencyFactory(provider);
            var builder = new ExtendedInitialStateComponentBuilder(component, factory);

            configuration.Invoke(builder);
            return new StateMachineCore(builder.InitialState, component);
        }));

        return this;
    }

    private void Remove(TKey key) => this.stateMachines.Remove(key);

    internal ConcurrentDictionary<TKey, Func<IStateMachineCore>> Build(IServiceProvider provider)
    {
        return new ConcurrentDictionary<TKey, Func<IStateMachineCore>>(this.stateMachines.ToDictionary(
            k => k.Key, 
            k => new Func<IStateMachineCore>(() => k.Value.Invoke(provider))));
    }
}

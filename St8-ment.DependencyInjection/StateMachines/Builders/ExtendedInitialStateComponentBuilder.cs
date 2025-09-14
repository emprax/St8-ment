using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedInitialStateComponentBuilder(IKeyValueStateComponent<StateId> parent, IDependencyFactory factory) : IExtendedInitialStateComponentBuilder
{
    internal StateId InitialState { get; private set; }

    public IExtendedStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IExtendedStateComponentBuilder> configuration)
    {
        var component = this.GetOrAdd(stateId);
        configuration.Invoke(new ExtendedStateComponentBuilder(component, factory));
        this.InitialState = stateId;

        return new ExtendedStateComponentCollectionBuilder(parent, factory);
    }

    public IExtendedStateComponentCollectionBuilder ForInitial(IExtendedStateConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new InvalidOperationException("When using state-configurations these should exist for the initial state.");
        }

        var component = this.GetOrAdd(configuration.StateId);
        configuration.Configure(new ExtendedStateComponentBuilder(component, factory));
        this.InitialState = configuration.StateId;

        return new ExtendedStateComponentCollectionBuilder(parent, factory);
    }

    private StateComponent GetOrAdd(StateId id)
    {
        if (!parent.TryGetValue(id, out var component) || component is null)
        {
            component = new StateComponent();
            parent.Add(id, component);
        }

        return (StateComponent)component;
    }
}

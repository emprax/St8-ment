using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines.Builders;

internal class InitialStateComponentBuilder(IKeyValueStateComponent<StateId> parent) : IInitialStateComponentBuilder
{
    internal StateId InitialState { get; private set; }

    public IStateComponentCollectionBuilder ForInitial(StateId stateId, Action<IStateComponentBuilder> configuration)
    {
        var component = this.GetOrAdd(stateId);
        configuration.Invoke(new StateComponentBuilder(component));
        this.InitialState = stateId;

        return new StateComponentCollectionBuilder(parent);
    }

    public IStateComponentCollectionBuilder ForInitial(IStateConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new InvalidOperationException("When using state-configurations these should exist for the initial state.");
        }

        var component = this.GetOrAdd(configuration.StateId);
        configuration.Configure(new StateComponentBuilder(component));
        this.InitialState = configuration.StateId;

        return new StateComponentCollectionBuilder(parent);
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

using St8Ment.StateMachines.Components;
using System;

namespace St8Ment.StateMachines.Builders;

internal class StateComponentCollectionBuilder(IKeyValueStateComponent<StateId> parent) : IStateComponentCollectionBuilder
{
    public IStateComponentCollectionBuilder For(StateId stateId, Action<IStateComponentBuilder> configuration)
    {
        var component = this.GetOrAdd(stateId);
        configuration.Invoke(new StateComponentBuilder(component));

        return this;
    }

    public IStateComponentCollectionBuilder For(IStateConfiguration configuration)
    {
        if (configuration != null)
        {
            var component = this.GetOrAdd(configuration.StateId);
            configuration.Configure(new StateComponentBuilder(component));
        }

        return this;
    }

    private StateComponent GetOrAdd(StateId id)
    {
        if (!parent.TryGetValue(id, out var component))
        {
            component = new StateComponent();
            parent.Add(id, component);
        }

        return (StateComponent)component!;
    }
}
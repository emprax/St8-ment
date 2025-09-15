using St8Ment.DependencyInjection.StateMachines.Abstractions;
using St8Ment.StateMachines.Components;
using System;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedStateComponentCollectionBuilder(IKeyValueStateComponent<StateId> parent, IDependencyFactory factory) : IExtendedStateComponentCollectionBuilder
{
    public IExtendedStateComponentCollectionBuilder For(StateId stateId, Action<IExtendedStateComponentBuilder> configuration)
    {
        var component = this.GetOrAdd(stateId);
        configuration.Invoke(new ExtendedStateComponentBuilder(component, factory));

        return this;
    }

    public IExtendedStateComponentCollectionBuilder For(IExtendedStateConfiguration configuration)
    {
        if (configuration != null)
        {
            var component = this.GetOrAdd(configuration.StateId);
            configuration.Configure(new ExtendedStateComponentBuilder(component, factory));
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
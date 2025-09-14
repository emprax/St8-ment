using System;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedStateComponentBuilder(IKeyValueStateComponent<string> parent, IDependencyFactory factory) : IExtendedStateComponentBuilder
{
    public IExtendedStateTransitionBuilder<TInput> On<TInput>()
    {
        var component = this.GetOrAdd<TInput>();
        return new ExtendedStateTransitionBuilder<TInput>(this, component, factory);
    }

    public IExtendedStateTransitionBuilder<object> OnDefault()
    {
        var component = this.GetOrAdd<object>();
        return new ExtendedStateTransitionBuilder<object>(this, component, factory);
    }

    private IItemStateComponent GetOrAdd<TInput>()
    {
        var key = typeof(TInput).FullName!;
        if (!parent.TryGetValue(key, out var component) || component is null)
        {
            component = new ActionsComponent();
            parent.Add(key, component);
        }

        return (IItemStateComponent)component;
    }
}

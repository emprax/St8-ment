using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines.Builders;

internal class StateComponentBuilder(IKeyValueStateComponent<string> parent) : IStateComponentBuilder
{
    public IStateTransitionBuilder<TInput> On<TInput>()
    {
        var component = this.GetOrAdd<TInput>();
        return new StateTransitionBuilder<TInput>(this, component);
    }

    public IStateTransitionBuilder<object> OnDefault()
    {
        var component = this.GetOrAdd<object>();
        return new StateTransitionBuilder<object>(this, component);
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
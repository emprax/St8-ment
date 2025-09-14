using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines.Builders;

internal class ExtendedCallbackComponentBuilder<TInput>(IExtendedStateComponentBuilder builder, IItemStateComponent parent) : IExtendedCallbackComponentBuilder<TInput>
{
    public IExtendedStateComponentBuilder To(StateId stateId)
    {
        parent.Add(new ResultComponent(stateId));
        return builder;
    }
}

using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines.Builders;

internal class CallbackComponentBuilder<TInput>(IStateComponentBuilder builder, IItemStateComponent parent) : ICallbackComponentBuilder<TInput>
{
    public IStateComponentBuilder To(StateId stateId)
    {
        parent.Add(new ResultComponent(stateId));
        return builder;
    }
}

using St8Ment.StateMachines.Components;
using System;

namespace St8Ment.StateMachines.Builders;

internal class SpecComponentBuilder<TInput>(IStateComponentBuilder builder, IItemStateComponent parent) : ISpecComponentBuilder<TInput>
{
    public ICallbackComponentBuilder<TInput> WithCallback(Func<ITransitionCallback<TInput>> callbackFactory)
    {
        var component = new CallbackComponent(callbackFactory.Invoke);
        parent.Add(component);

        return new CallbackComponentBuilder<TInput>(builder, component);
    }

    public ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback) => this.WithCallback(() => callback);

    public IStateComponentBuilder To(StateId stateId)
    {
        parent.Add(new ResultComponent(stateId));
        return builder;
    }
}
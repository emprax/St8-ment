using System;

namespace St8Ment.StateMachines.Builders;

public interface ISpecComponentBuilder<TInput>
{
    ICallbackComponentBuilder<TInput> WithCallback(Func<ITransitionCallback<TInput>> callbackFactory);

    ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

    IStateComponentBuilder To(StateId stateId);
}
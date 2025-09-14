using St8Ment.StateMachines;
using System;

namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedSpecComponentBuilder<TInput>
{
    IExtendedCallbackComponentBuilder<TInput> WithCallback(Func<IDependencyFactory, ITransitionCallback<TInput>> callbackFactory);

    IExtendedCallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

    IExtendedCallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput>;

    IExtendedCallbackComponentBuilder<TInput> WithCallback(Type callbackType);

    IExtendedStateComponentBuilder To(StateId stateId);
}

using System;
using St8Ment.StateMachines;

namespace St8Ment.DependencyInjection.StateMachines.Builders
{
    public interface ISpecComponentBuilder<TInput>
    {
        ICallbackComponentBuilder<TInput> WithCallback(Func<IServiceProvider, ITransitionCallback<TInput>> callbackFactory);

        ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

        ICallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput>;

        ICallbackComponentBuilder<TInput> WithCallback(Type callbackType);

        IStateComponentBuilder To(StateId stateId);
    }
}

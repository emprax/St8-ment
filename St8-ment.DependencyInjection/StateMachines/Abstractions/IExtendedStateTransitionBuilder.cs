using SpeciFire;
using St8Ment.StateMachines;
using System;
using System.Linq.Expressions;

namespace St8Ment.DependencyInjection.StateMachines.Abstractions;

public interface IExtendedStateTransitionBuilder<TInput>
{
    IExtendedSpecComponentBuilder<TInput> WithGuard(Func<IDependencyFactory, ISpec<TInput>> guardFactory);
    
    IExtendedSpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec);

    IExtendedSpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression);

    IExtendedSpecComponentBuilder<TInput> WithGuard<TSpec>() where TSpec : class, ISpec<TInput>;

    IExtendedSpecComponentBuilder<TInput> WithGuard(Type specType);

    IExtendedCallbackComponentBuilder<TInput> WithCallback(Func<IDependencyFactory, ITransitionCallback<TInput>> callbackFactory);

    IExtendedCallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

    IExtendedCallbackComponentBuilder<TInput> WithCallback<TCallback>() where TCallback : class, ITransitionCallback<TInput>;

    IExtendedCallbackComponentBuilder<TInput> WithCallback(Type callbackType);

    IExtendedStateComponentBuilder To(StateId stateId);
}

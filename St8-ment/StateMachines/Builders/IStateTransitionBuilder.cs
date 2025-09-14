using System;
using System.Linq.Expressions;
using SpeciFire;
using St8Ment.StateMachines;

namespace St8Ment.StateMachines.Builders;

public interface IStateTransitionBuilder<TInput>
{
    ISpecComponentBuilder<TInput> WithGuard(Func<ISpec<TInput>> guardFactory);
    
    ISpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec);

    ISpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression);

    ICallbackComponentBuilder<TInput> WithCallback(Func<ITransitionCallback<TInput>> callbackFactory);

    ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback);

    IStateComponentBuilder To(StateId stateId);
}
using System;
using System.Linq;
using System.Linq.Expressions;
using SpeciFire;
using SpeciFire.Specifications;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;

namespace St8Ment.StateMachines.Builders;

internal class StateTransitionBuilder<TInput>(IStateComponentBuilder builder, IItemStateComponent parent) : IStateTransitionBuilder<TInput>
{
    public IStateComponentBuilder To(StateId stateId)
    {
        parent.Add(new ResultComponent(stateId));
        return builder;
    }

    public ICallbackComponentBuilder<TInput> WithCallback(Func<ITransitionCallback<TInput>> callbackFactory)
    {
        var component = new CallbackComponent(callbackFactory.Invoke);
        parent.Add(component);

        return new CallbackComponentBuilder<TInput>(builder, component);
    }

    public ICallbackComponentBuilder<TInput> WithCallback(ITransitionCallback<TInput> callback) => this.WithCallback(() => callback);

    public ISpecComponentBuilder<TInput> WithGuard(Func<ISpec<TInput>> guardFactory)
    {
        var component = new SpecComponent(guardFactory.Invoke);
        parent.Add(component);

        return new SpecComponentBuilder<TInput>(builder, component);
    }

    public ISpecComponentBuilder<TInput> WithGuard(ISpec<TInput> spec) => this.WithGuard(() => spec);

    public ISpecComponentBuilder<TInput> WithGuard(Expression<Func<TInput, bool>> expression) => this.WithGuard(new ExpressionSpec<TInput>(expression));
}
﻿using St8_ment.V1;
using System;

namespace St8_ment.DependencyInjection.V1
{
    public interface IStateMachineBuilder<TContext> where TContext : IStateContext<TContext>
    {
        IStateMachineBuilder<TContext> For<TState>(StateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>;

        IStateMachineBuilder<TContext> For<TState>(Action<IStateConfigurator<TState, TContext>> configuration) where TState : class, IState<TContext>;

        IStateMachineBuilder<TContext> For<TState>() where TState : class, IState<TContext>;

        IStateMachine<TContext> Build(IServiceProvider provider);
    }
}
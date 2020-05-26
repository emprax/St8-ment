using System;

namespace St8_ment.DependencyInjection
{
    public interface IStateMachineBuilder<TContext> where TContext : IStateContext
    {
        IStateMachineBuilder<TContext> For<TState>(IStateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>;

        IStateMachineBuilder<TContext> For<TState>(Action<IStateConfigurator<TState, TContext>> configuration) where TState : class, IState<TContext>;

        IStateMachine<TContext> Build(IServiceProvider provider);
    }
}
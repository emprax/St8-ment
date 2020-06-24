using St8_ment.V2;
using System;
using System.Collections;
using System.Linq;

namespace St8_ment.DependencyInjection.V2
{
    public interface IStateMachineBuilder<TContext> where TContext : IStateContext<TContext>
    {
        IStateMachineBuilder<TContext> For<TState>(StateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>;

        IStateMachineBuilder<TContext> For<TState>(Action<IStateConfigurator<TState, TContext>> configuration) where TState : class, IState<TContext>;

        IStateMachineBuilder<TContext> For<TState>() where TState : class, IState<TContext>;

        IStateMachine<TContext> Build(IServiceProvider provider);
    }
}

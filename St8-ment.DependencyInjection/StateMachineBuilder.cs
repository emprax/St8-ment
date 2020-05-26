using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace St8_ment.DependencyInjection
{
    public class StateMachineBuilder<TContext> : IStateMachineBuilder<TContext> where TContext : IStateContext
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<int, Func<IServiceProvider, IState<TContext>>> states;

        public StateMachineBuilder(IServiceCollection services)
        {
            this.services = services;
            this.states = new Dictionary<int, Func<IServiceProvider, IState<TContext>>>();
        }

        public IStateMachineBuilder<TContext> For<TState>(IStateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>
        {
            var result = configuration.Build(this.services);
            this.states.Add(typeof(TState).GetHashCode(), result);
            return this;
        }

        public IStateMachineBuilder<TContext> For<TState>(Action<IStateConfigurator<TState, TContext>> configuration) where TState : class, IState<TContext>
        {
            var result = new LambdaStateConfiguration<TState, TContext>(configuration).Build(this.services);
            this.states.Add(typeof(TState).GetHashCode(), result);
            return this;
        }

        public IStateMachine<TContext> Build(IServiceProvider provider)
        {
            return new StateMachine<TContext>(this.states.ToDictionary(x => x.Key, x => x.Value.Invoke(provider)));
        }
    }
}
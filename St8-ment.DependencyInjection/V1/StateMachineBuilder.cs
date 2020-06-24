using Microsoft.Extensions.DependencyInjection;
using St8_ment.V1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace St8_ment.DependencyInjection.V1
{
    public class StateMachineBuilder<TContext> : IStateMachineBuilder<TContext> where TContext : IStateContext<TContext>
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<int, Func<IServiceProvider, TContext, IState<TContext>>> states;

        public StateMachineBuilder(IServiceCollection services)
        {
            this.services = services;
            this.states = new Dictionary<int, Func<IServiceProvider, TContext, IState<TContext>>>();
        }

        public IStateMachineBuilder<TContext> For<TState>(StateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>
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

        public IStateMachineBuilder<TContext> For<TState>() where TState : class, IState<TContext>
        {
            var result = new LambdaStateConfiguration<TState, TContext>(_ => { }).Build(this.services);
            this.states.Add(typeof(TState).GetHashCode(), result);
            return this;
        }

        public IStateMachine<TContext> Build(IServiceProvider provider)
        {
            return new StateMachine<TContext>(this.states.ToDictionary(x => x.Key, x => new Func<TContext, IState<TContext>>(context =>
            {
                var state = x.Value.Invoke(provider, context);
                return state;
            })));
        }
    }
}
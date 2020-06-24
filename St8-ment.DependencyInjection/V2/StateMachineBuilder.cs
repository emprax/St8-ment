using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace St8_ment.DependencyInjection.V2
{
    public class StateMachineBuilder<TContext> : IStateMachineBuilder<TContext> where TContext : IStateContext<TContext>
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<int, Func<IServiceProvider, IStateTransitionerProvider>> providers;

        public StateMachineBuilder(IServiceCollection services)
        {
            this.services = services;
            this.providers = new Dictionary<int, Func<IServiceProvider, IStateTransitionerProvider>>();
        }

        public IStateMachine<TContext> Build(IServiceProvider provider)
        {
            return new StateMachine<TContext>(this.providers.ToDictionary(
                stateProvider => stateProvider.Key,
                stateProvider => stateProvider.Value.Invoke(provider)));
        }

        public IStateMachineBuilder<TContext> For<TState>(StateConfiguration<TState, TContext> configuration) where TState : class, IState<TContext>
        {
            var function = configuration.Build(this.services);
            this.providers.Add(typeof(TState).GetHashCode(), function);
            return this;
        }

        public IStateMachineBuilder<TContext> For<TState>(Action<IStateConfigurator<TState, TContext>> configuration) where TState : class, IState<TContext>
        {
            var function = new LambdaStateConfiguration<TState, TContext>(configuration).Build(this.services);
            this.providers.Add(typeof(TState).GetHashCode(), function);
            return this;
        }

        public IStateMachineBuilder<TContext> For<TState>() where TState : class, IState<TContext>
        {
            var function = new LambdaStateConfiguration<TState, TContext>(_ => { }).Build(this.services);
            this.providers.Add(typeof(TState).GetHashCode(), function);
            return this;
        }
    }
}

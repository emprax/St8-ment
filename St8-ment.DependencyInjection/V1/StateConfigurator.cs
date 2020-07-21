using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using St8_ment.V1;

namespace St8_ment.DependencyInjection.V1
{
    public class StateConfigurator<TState, TContext> : IStateConfigurator<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Type> actions;
        private readonly IServiceCollection services;

        public StateConfigurator(IServiceCollection services, IDictionary<int, Type> actions)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.actions = actions ?? throw new ArgumentNullException(nameof(actions));
        }

        public IActionConfiguration<TAction, TState> On<TAction>() where TAction : IAction
        {
            return new ActionConfiguration<TAction, TState>(this.services, this.actions);
        }
    }
}
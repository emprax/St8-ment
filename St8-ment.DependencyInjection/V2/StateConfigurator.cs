using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;
using System.Collections.Generic;

namespace St8_ment.DependencyInjection.V2
{
    public class StateConfigurator<TState, TContext> : IStateConfigurator<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IDictionary<int, Type> actions;
        private readonly IServiceCollection services;

        public StateConfigurator(IServiceCollection services, IDictionary<int, Type> actions)
        {
            this.services = services;
            this.actions = actions;
        }

        public IActionConfiguration<TAction, TState, TContext> On<TAction>() where TAction : IAction
        {
            return new ActionConfiguration<TAction, TState, TContext>(this.services, this.actions);
        }
    }
}

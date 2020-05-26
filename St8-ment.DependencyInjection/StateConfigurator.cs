using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace St8_ment.DependencyInjection
{
    public class StateConfigurator<TState, TContext> : IStateConfigurator<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        private readonly IList<Type> actions;
        private IServiceCollection services;

        public StateConfigurator(IServiceCollection services, IList<Type> actions)
        {
            this.services = services;
            this.actions = actions;
        }

        public IActionConfiguration<TAction, TState> On<TAction>() where TAction : IAction<TState>
        {
            return new ActionConfiguration<TAction, TState>(this.services, this.actions);
        }
    }
}
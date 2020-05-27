using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace St8_ment.DependencyInjection
{
    public class ActionConfiguration<TAction, TState> : IActionConfiguration<TAction, TState> 
        where TAction : IAction
        where TState : class, IState
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<int, Type> actions;

        public ActionConfiguration(IServiceCollection services, IDictionary<int, Type> actions)
        {
            this.services = services;
            this.actions = actions;
        }

        public void Transition<TTransition>() where TTransition : class, IStateTransition<StateTransaction<TAction, TState>>
        {
            this.services.AddTransient<IStateTransition<StateTransaction<TAction, TState>>, TTransition>();
            this.actions.Add(typeof(StateTransaction<TAction, TState>).GetHashCode(), typeof(IStateTransition<StateTransaction<TAction, TState>>));
        }
    }
}
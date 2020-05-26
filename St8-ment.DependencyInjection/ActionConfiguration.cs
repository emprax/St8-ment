using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace St8_ment.DependencyInjection
{
    public class ActionConfiguration<TAction, TState> : IActionConfiguration<TAction, TState> 
        where TAction : IAction<TState>
        where TState : class, IState
    {
        private readonly IServiceCollection services;
        private readonly IList<Type> actions;

        public ActionConfiguration(IServiceCollection services, IList<Type> actions)
        {
            this.services = services;
            this.actions = actions;
        }

        public void Transition<TTransition>() where TTransition : class, IStateTransition<StateTransaction<TAction, TState>>
        {
            this.services.AddTransient<IStateTransition<StateTransaction<TAction, TState>>, TTransition>();
            this.actions.Add(typeof(TAction));
        }
    }
}
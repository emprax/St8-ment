using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;
using System.Collections.Generic;

namespace St8_ment.DependencyInjection.V2
{
    public class ActionConfiguration<TAction, TState, TContext> : IActionConfiguration<TAction, TState, TContext>
        where TAction : IAction
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IDictionary<int, Type> transitioners;

        public ActionConfiguration(IServiceCollection serviceCollection, IDictionary<int, Type> transitioners)
        {
            this.serviceCollection = serviceCollection;
            this.transitioners = transitioners;
        }

        public void Transition<TTransitioner>() where TTransitioner : class, IStateTransitioner<TAction, TState, TContext>
        {
            this.serviceCollection.AddTransient<IStateTransitioner<TAction, TState, TContext>, TTransitioner>();
            this.transitioners.Add(typeof(StateTransaction<TAction, TState>).GetHashCode(), typeof(IStateTransitioner<TAction, TState, TContext>));
        }
    }
}

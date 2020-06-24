using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace St8_ment.DependencyInjection.V2
{
    public abstract class StateConfiguration<TState, TContext> : IStateConfiguration<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        protected abstract void Configure(IStateConfigurator<TState, TContext> configurator);

        public Func<IServiceProvider, IStateTransitionerProvider<TState, TContext>> Build(IServiceCollection services)
        {
            var actions = new Dictionary<int, Type>();
            this.Configure(new StateConfigurator<TState, TContext>(services, actions));

            return new Func<IServiceProvider, IStateTransitionerProvider<TState, TContext>>(provider =>
            {
                return new StateTransitionerProvider<TState, TContext>(actions.ToDictionary(
                    actionConfiguration => actionConfiguration.Key,
                    actionConfiguration => new Func<IStateTransitionerMarker>(() => provider.GetService(actionConfiguration.Value) as IStateTransitionerMarker)));
            });
        }
    }
}

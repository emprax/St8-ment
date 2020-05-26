using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace St8_ment.DependencyInjection
{
    public abstract class StateConfiguration<TState, TContext> : IStateConfiguration<TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        protected abstract void Configure(IStateConfigurator<TState, TContext> configurator);

        public Func<IServiceProvider, TState> Build(IServiceCollection services)
        {
            var actions = new List<Type>();
            this.Configure(new StateConfigurator<TState, TContext>(services, actions));

            return new Func<IServiceProvider, TState>(provider =>
            {
                var constructor = typeof(TState)
                    .GetConstructors()
                    .FirstOrDefault(x => x.GetParameters()
                    .Any(y => y.ParameterType == typeof(IStateTransitionProvider)));

                return constructor.Invoke(constructor.GetParameters().Select(x => x.ParameterType == typeof(IStateTransitionProvider)
                    ? new StateTransitionProvider<TState, TContext>(actions.ToDictionary(type => type.GetHashCode(), type => new Func<IStateTransitionMarker>(() => provider.GetService(type) as IStateTransitionMarker)))
                    : provider.GetService(x.ParameterType)).ToArray()) as TState;
            });
        }
    }
}
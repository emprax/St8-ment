using Microsoft.Extensions.DependencyInjection;
using St8_ment.V1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace St8_ment.DependencyInjection.V1
{
    public abstract class StateConfiguration<TState, TContext> : IStateConfiguration<TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        protected abstract void Configure(IStateConfigurator<TState, TContext> configurator);

        public Func<IServiceProvider, TContext, TState> Build(IServiceCollection services)
        {
            var actions = new Dictionary<int, Type>();
            this.Configure(new StateConfigurator<TState, TContext>(services, actions));

            var constructor = typeof(TState)
                .GetConstructors()
                .FirstOrDefault(x =>
                    x.GetParameters().Any(y => y.ParameterType == typeof(IStateTransitionerProvider)) &&
                    x.GetParameters().Any(y => y.ParameterType == typeof(TContext)));

            return new Func<IServiceProvider, TContext, TState>((provider, context) => constructor
                .Invoke(constructor.GetParameters().Select(x => 
                {
                    if (x.ParameterType == typeof(IStateTransitionerProvider))
                    {
                        return new StateTransitionerProvider<TState, TContext>(actions.ToDictionary(
                            pair => pair.Key,
                            pair => new Func<IStateTransitionerMarker>(() => provider.GetService(pair.Value) as IStateTransitionerMarker)));
                    }

                    return x.ParameterType != typeof(TContext) 
                        ? provider.GetService(x.ParameterType)
                        : context;
                })
                .ToArray()) as TState);
        }
    }
}
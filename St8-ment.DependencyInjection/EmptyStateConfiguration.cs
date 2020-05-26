using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace St8_ment.DependencyInjection
{
    public class EmptyStateConfiguration<TState, TContext> : IStateConfiguration<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        public Func<IServiceProvider, TContext, TState> Build(IServiceCollection services)
        {
            var constructor = typeof(TState)
                .GetConstructors()
                .FirstOrDefault(x =>
                    x.GetParameters().Any(y => y.ParameterType == typeof(IStateTransitionProvider)) &&
                    x.GetParameters().Any(y => y.ParameterType == typeof(TContext)));

            return new Func<IServiceProvider, TContext, TState>((provider, context) => constructor
                .Invoke(constructor.GetParameters().Select(x =>
                {
                    if (x.ParameterType == typeof(IStateTransitionProvider))
                    {
                        return new StateTransitionProvider<TState, TContext>(new Dictionary<int, Func<IStateTransitionMarker>>());
                    }

                    return x.ParameterType != typeof(TContext)
                        ? provider.GetService(x.ParameterType)
                        : context;
                })
                .ToArray()) as TState);
        }
    }
}
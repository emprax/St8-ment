using Microsoft.Extensions.DependencyInjection;
using St8_ment.V1;
using System;

namespace St8_ment.DependencyInjection.V1
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateMachine<TContext>(this IServiceCollection services, Action<IStateMachineBuilder<TContext>> builder) where TContext : IStateContext<TContext>
        {
            var machineBuilder = new StateMachineBuilder<TContext>(services);
            builder?.Invoke(machineBuilder);
            services.AddTransient(provider => machineBuilder.Build(provider));
            return services;
        }
    }
}

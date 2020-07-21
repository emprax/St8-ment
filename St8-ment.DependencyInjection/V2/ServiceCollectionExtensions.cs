using Microsoft.Extensions.DependencyInjection;
using St8_ment.V2;
using System;

namespace St8_ment.DependencyInjection.V2
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateMachineV2<TContext>(this IServiceCollection services, Action<IStateMachineBuilder<TContext>> builder) where TContext : class, IStateContext<TContext>
        {
            var machineBuilder = new StateMachineBuilder<TContext>(services);
            builder?.Invoke(machineBuilder);
            services.AddTransient(provider => machineBuilder.Build(provider));
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;

namespace St8_ment.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateMachine<TContext>(this IServiceCollection services, Action<IStateMachineBuilder<TContext>> builder) where TContext : IStateContext<TContext>
        {
            var machineBuilder = new StateMachineBuilder<TContext>(services);
            builder.Invoke(machineBuilder);
            services.AddTransient(provider => machineBuilder.Build(provider));
            return services;
        }
    }
}

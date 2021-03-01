using System;
using Microsoft.Extensions.DependencyInjection;
using St8_ment.DependencyInjection.StateMachines.Builders;
using St8_ment.StateMachines;
using St8_ment.StateMachines.Components;

namespace St8_ment.DependencyInjection.StateMachines
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateMachine(this IServiceCollection services, Action<IInitialStateComponentBuilder, IServiceProvider> configuration)
        {
            return services
                .AddTransient<IStateMachine, StateMachine>()
                .AddTransient<IStateMachineCore>(provider => 
                {
                    var component = new StateComponentCollection();
                    var builder = new InitialStateComponentBuilder(component, provider);
                    configuration?.Invoke(builder, provider);
                    
                    return new StateMachineCore(builder.InitialState, component);
                });
        }

        public static IServiceCollection AddStateMachineFactory<TKey>(this IServiceCollection services, Action<IStateMachineFactoryBuilder<TKey>, IServiceProvider> configuration)
        {
            return services.AddSingleton<IStateMachineFactory<TKey>>(provider =>
            {
                var builder = new StateMachineFactoryBuilder<TKey>();
                configuration?.Invoke(builder, provider);
                return new StateMachineFactory<TKey>(builder.Build(provider));
            });
        }
    }
}

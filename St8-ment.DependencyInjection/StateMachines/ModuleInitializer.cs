using System;
using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.StateMachines.Abstractions;
using St8Ment.DependencyInjection.StateMachines.Builders;
using St8Ment.StateMachines;
using St8Ment.StateMachines.Components;

namespace St8Ment.DependencyInjection.StateMachines;

public static class ModuleInitializer
{
    public static IServiceCollection AddStateMachine(this IServiceCollection services, Action<IExtendedInitialStateComponentBuilder, IDependencyFactory> configuration)
    {
        return services
            .AddTransient<IStateMachine, StateMachine>()
            .AddTransient<IStateMachineCore>(provider => 
            {
                var factory = new DependencyFactory(provider);
                var component = new StateComponentCollection();
                var builder = new ExtendedInitialStateComponentBuilder(component, factory);

                configuration?.Invoke(builder, factory);
                return new StateMachineCore(builder.InitialState, component);
            });
    }

    public static IServiceCollection AddStateMachineFactory<TKey>(this IServiceCollection services, Action<IExtendedStateMachineFactoryBuilder<TKey>, IDependencyFactory> configuration)
        where TKey : notnull
    {
        return services.AddSingleton<IStateMachineProvider<TKey>>(provider =>
        {
            var builder = new ExtendedStateMachineFactoryBuilder<TKey>();
            configuration?.Invoke(builder, new DependencyFactory(provider));
            return new StateMachineProvider<TKey>(builder.Build(provider));
        });
    }
}
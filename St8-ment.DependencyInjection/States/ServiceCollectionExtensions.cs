using Microsoft.Extensions.DependencyInjection;
using St8Ment.DependencyInjection.States.Forge;
using St8Ment.States;
using System;

namespace St8Ment.DependencyInjection.States
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateReducer<TSubject>(this IServiceCollection services, Action<IStateReducerBuilder<TSubject>, DependencyProvider> configuration)
            where TSubject : ExtendedStateSubject<TSubject>
        {
            return services
                .AddTransient<IStateReducer<TSubject>, StateReducer<TSubject>>()
                .AddSingleton(provider => 
                {
                    var builder = new StateReducerBuilder<TSubject>();
                    configuration?.Invoke(builder, provider.GetService);
                    return builder.Build(provider.GetService);
                });
        }

        public static IServiceCollection AddStateReducerFactory<TKey, TSubject>(this IServiceCollection services, Action<IStateReducerFactoryBuilder<TKey, TSubject>, DependencyProvider> configuration)
            where TSubject : ExtendedStateSubject<TSubject>
        {
            return services.AddSingleton<IStateReducerFactory<TKey, TSubject>>(provider =>
            {
                var builder = new StateReducerFactoryBuilder<TKey, TSubject>();
                configuration?.Invoke(builder, provider.GetService);
                return new StateReducerFactory<TKey, TSubject>(builder.Build(), provider.GetService);
            });
        }

        public static IServiceCollection AddStateForge(this IServiceCollection services, Action<IStateForgeBuilder, DependencyProvider> configuration)
        {
            return services.AddSingleton(provider =>
            {
                var builder = new StateForgeBuilder(provider.GetService);
                configuration?.Invoke(builder, provider.GetService);
                return builder.Build();
            });
        }
    }
}

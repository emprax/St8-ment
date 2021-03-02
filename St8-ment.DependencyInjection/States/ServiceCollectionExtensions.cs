using System;
using Microsoft.Extensions.DependencyInjection;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateReducer<TSubject>(this IServiceCollection services, Action<IStateReducerBuilder<TSubject>, IServiceProvider> configuration)
            where TSubject : class, IStateSubject<TSubject>
        {
            return services
                .AddTransient<IStateReducer<TSubject>, StateReducer<TSubject>>()
                .AddSingleton(provider => 
                {
                    var builder = new StateReducerBuilder<TSubject>();
                    configuration?.Invoke(builder, provider);
                    return builder.Build(provider);
                });
        }

        public static IServiceCollection AddStateReducerFactory<TKey, TSubject>(this IServiceCollection services, Action<IStateReducerFactoryBuilder<TKey, TSubject>, IServiceProvider> configuration)
            where TSubject : class, IStateSubject<TSubject>
        {
            return services.AddSingleton<IStateReducerFactory<TKey, TSubject>>(provider =>
            {
                var builder = new StateReducerFactoryBuilder<TKey, TSubject>();
                configuration?.Invoke(builder, provider);
                return new StateReducerFactory<TKey, TSubject>(builder.Build(provider));
            });
        }
    }
}

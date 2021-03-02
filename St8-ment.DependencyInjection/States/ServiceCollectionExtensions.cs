using System;
using Microsoft.Extensions.DependencyInjection;
using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStateReducer<TContext>(this IServiceCollection services, Action<IStateReducerBuilder<TContext>, IServiceProvider> configuration)
            where TContext : class, IStateContext<TContext>
        {
            return services
                .AddTransient<IStateReducer<TContext>, StateReducer<TContext>>()
                .AddSingleton(provider => 
                {
                    var builder = new StateReducerBuilder<TContext>();
                    configuration?.Invoke(builder, provider);
                    return builder.Build(provider);
                });
        }

        public static IServiceCollection AddStateReducerFactory<TKey, TContext>(this IServiceCollection services, Action<IStateReducerFactoryBuilder<TKey, TContext>, IServiceProvider> configuration)
            where TContext : class, IStateContext<TContext>
        {
            return services.AddSingleton<IStateReducerFactory<TKey, TContext>>(provider =>
            {
                var builder = new StateReducerFactoryBuilder<TKey, TContext>();
                configuration?.Invoke(builder, provider);
                return new StateReducerFactory<TKey, TContext>(builder.Build(provider));
            });
        }
    }
}

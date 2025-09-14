using Microsoft.Extensions.DependencyInjection;
using St8ment.Core;
using St8ment.DependencyInjection.Abstractions;
using St8ment.DependencyInjection.Builders;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Abstractions.Factories;
using St8Ment.States.Factories;

namespace St8ment.DependencyInjection;

public static class ModuleInitializer
{
    public static IServiceCollection AddSt8ment<TSubject>(this IServiceCollection services, ServiceLifetime lifetime, Action<IExtendedStateContextsBuilder<TSubject>> action)
        where TSubject : class, IStateSubject<TSubject>
    {
        services.Add(new(typeof(IStateContextProvider<TSubject>), lifetime: lifetime, factory: provider =>
        {
            var dictionary = new Dictionary<string, IStateContext<TSubject>>();
            var factory = new DependencyFactory(provider);

            action.Invoke(new ExtendedStateContextsBuilder<TSubject>(dictionary, factory));
            return new StateContextProvider<TSubject>(dictionary);
        }));

        services.Add(new(typeof(IStateReducerFactory<TSubject>), typeof(StateReducerFactory<TSubject>), lifetime));
        return services;
    }

    public static IServiceCollection AddSt8ment<TSubject>(this IServiceCollection services, Action<IExtendedStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>
        => services.AddSt8ment(ServiceLifetime.Singleton, action);

    public static IServiceCollection AddSt8ment(this IServiceCollection services, ServiceLifetime lifeTime, Action<IExtendedStatesBuilder> action)
    {
        services.Add(new(serviceType: typeof(IStateContextsProvider), lifetime: lifeTime, factory: provider =>
        {
            var dictionary = new Dictionary<int, IStateContextProvider>();
            var factory = new DependencyFactory(provider);

            action.Invoke(new ExtendedStatesBuilder(factory, dictionary));
            return new StateContextsProvider(dictionary);
        }));

        services.Add(new(serviceType: typeof(IStatesFactory), implementationType: typeof(StateReducerFactory), lifetime: lifeTime));
        return services;
    }

    public static IServiceCollection AddSt8ment(this IServiceCollection services, Action<IExtendedStatesBuilder> action)
        => services.AddSt8ment(ServiceLifetime.Singleton, action);
}
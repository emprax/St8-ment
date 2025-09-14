using St8ment.Core;
using St8ment.DependencyInjection.Abstractions;
using St8Ment.States.Abstractions.Core;

namespace St8ment.DependencyInjection.Builders;

#pragma warning disable IDE0290 // Use primary constructor
public class ExtendedStatesBuilder : IExtendedStatesBuilder
{
    private readonly IDependencyFactory factory;
    private readonly IDictionary<int, IStateContextProvider> providers;

    public ExtendedStatesBuilder(IDependencyFactory factory, IDictionary<int, IStateContextProvider> providers)
    {
        this.factory = factory;
        this.providers = providers;
    }

    public IExtendedStatesBuilder For<TSubject>(Action<IExtendedStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>
    {
        var dictionary = new Dictionary<string, IStateContext<TSubject>>();
        action.Invoke(new ExtendedStateContextsBuilder<TSubject>(dictionary, this.factory));

        var provider = new StateContextProvider<TSubject>(dictionary);
        var id = TypeId.Get<TSubject>();

        if (!this.providers.TryAdd(id, provider))
        {
            this.providers[id] = provider;
        }

        return this;
    }
}
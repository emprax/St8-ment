using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Abstractions.Factories;
using System;
using System.Collections.Generic;

namespace St8Ment.States.Builders;

public class StatesBuilder(IStateContextProviderFactory factory, IDictionary<int, IStateContextProvider> providers) : IStatesBuilder
{
    public IStatesBuilder For<TSubject>(Action<IStateContextsBuilder<TSubject>> action) where TSubject : class, IStateSubject<TSubject>
    {
        var provider = factory.Create(action);
        var id = TypeId.Get<TSubject>();

        if (!providers.TryAdd(id, provider))
        {
            providers[id] = provider;
        }

        return this;
    }
}
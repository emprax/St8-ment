using St8Ment.States.Abstractions.Core;
using System.Collections.Generic;

namespace St8Ment.States.Core;

public class StateContextsProvider(IDictionary<int, IStateContextProvider> providers) : IStateContextsProvider
{
    public IStateContextProvider<TSubject>? Get<TSubject>() where TSubject : class, IStateSubject<TSubject>
    {
        var id = TypeId.Get<TSubject>();
        providers.TryGetValue(id, out var provider);

        return provider as IStateContextProvider<TSubject>;
    }
}
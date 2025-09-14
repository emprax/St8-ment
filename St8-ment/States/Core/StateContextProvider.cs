using St8Ment.States.Abstractions.Core;
using System.Collections.Generic;

namespace St8Ment.States.Core;

public class StateContextProvider<TSubject>(IReadOnlyDictionary<string, IStateContext<TSubject>> contexts) : IStateContextProvider<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    public IStateContext<TSubject>? Get(StateId state)
    {
        contexts.TryGetValue(state.Value, out var context);
        return context;
    }
}
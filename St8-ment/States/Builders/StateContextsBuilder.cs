using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Core;
using System;
using System.Collections.Generic;

namespace St8Ment.States.Builders;

public class StateContextsBuilder<TSubject>(IDictionary<string, IStateContext<TSubject>> contexts) : IStateContextsBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    public IStateContextsBuilder<TSubject> State(StateId state, Action<IStateContextBuilder<TSubject>> action)
    {
        var dictionary = new Dictionary<int, IActionHandler<TSubject>>();
        action.Invoke(new StateContextBuilder<TSubject>(dictionary));

        var context = new StateContext<TSubject>(dictionary);
        if (!contexts.TryAdd(state.Value, context))
        {
            contexts[state.Value] = context;
        }

        return this;
    }
}
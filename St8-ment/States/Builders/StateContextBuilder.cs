using St8Ment.States.Abstractions.Builders;
using St8Ment.States.Abstractions.Core;
using System.Collections.Generic;

namespace St8Ment.States.Builders;

public class StateContextBuilder<TSubject>(IDictionary<int, IActionHandler<TSubject>> handlers) : IStateContextBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    public IStateContextBuilder<TSubject> Action<TAction, THandler>(THandler handler)
        where TAction : IAction<TSubject>
        where THandler : class, IActionHandler<TSubject, TAction>
    {
        var id = TypeId.Get<TAction>();
        if (!handlers.TryAdd(id, handler))
        {
            handlers[id] = handler;
        }

        return this;
    }
}
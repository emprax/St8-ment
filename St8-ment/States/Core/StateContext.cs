using St8Ment.States.Abstractions.Core;
using System.Collections.Generic;

namespace St8Ment.States.Core;

public class StateContext<TSubject>(IReadOnlyDictionary<int, IActionHandler<TSubject>> handlers) : IStateContext<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    public IActionHandler<TSubject, TAction>? Get<TAction>() where TAction : IAction<TSubject>
    {
        var id = TypeId.Get<TAction>();
        handlers.TryGetValue(id, out var handler);

        return handler as IActionHandler<TSubject, TAction>;
    }
}
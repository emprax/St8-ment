using St8Ment.States.Abstractions.Core;

namespace St8Ment.States.Abstractions.Builders;

public interface IStateContextBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IStateContextBuilder<TSubject> Action<TAction, THandler>(THandler handler)
        where TAction : IAction<TSubject>
        where THandler : class, IActionHandler<TSubject, TAction>;
}
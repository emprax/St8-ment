using St8Ment.States.Abstractions.Core;

namespace St8Ment.DependencyInjection.States.Abstractions;

public interface IExtendedStateContextBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
{
    IExtendedStateContextBuilder<TSubject> Action<TAction, THandler>()
        where TAction : IAction<TSubject>
        where THandler : IActionHandler<TSubject, TAction>;
}
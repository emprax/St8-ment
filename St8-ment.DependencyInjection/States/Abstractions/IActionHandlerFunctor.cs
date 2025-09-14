using St8Ment.States.Abstractions.Core;

namespace St8ment.DependencyInjection.Abstractions;

public interface IActionHandlerFunctor<TSubject, TAction>
    where TSubject : class, IStateSubject<TSubject>
    where TAction : IAction<TSubject>
{
    IActionHandler<TSubject, TAction> Invoke(IDependencyFactory factory);
}
using St8ment.DependencyInjection.Abstractions;
using St8Ment.States.Abstractions.Core;

namespace St8ment.DependencyInjection.Builders;

#pragma warning disable IDE0290 // Use primary constructor
public class ActionFactoryHandler<TSubject, TAction> : IActionHandler<TSubject, TAction>
    where TSubject : class, IStateSubject<TSubject>
    where TAction : IAction<TSubject>
{
    private readonly IDependencyFactory factory;
    private readonly IActionHandlerFunctor<TSubject, TAction> functor;

    public ActionFactoryHandler(IDependencyFactory factory, IActionHandlerFunctor<TSubject, TAction> functor)
    {
        this.factory = factory;
        this.functor = functor;
    }

    public Task ExecuteAsync(TAction action, IStateHandle<TSubject> handle, CancellationToken cancellationToken)
    {
        var handler = this.functor.Invoke(this.factory);
        return handler.ExecuteAsync(action, handle, cancellationToken);
    }
}
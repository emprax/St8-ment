using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IActionBuilder<TAction, TSubject>
        where TAction : class, IAction
        where TSubject : class, IStateSubject<TSubject>
    {
        IStateBuilder<TSubject> Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>;
    }
}

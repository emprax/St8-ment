using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IActionBuilder<out TAction, TSubject>
        where TAction : class, IAction
        where TSubject : ExtendedStateSubject<TSubject>
    {
        void Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>;
    }
}

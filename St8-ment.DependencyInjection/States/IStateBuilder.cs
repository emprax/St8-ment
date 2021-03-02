using St8Ment.States;

namespace St8Ment.DependencyInjection.States
{
    public interface IStateBuilder<TSubject> where TSubject : class, IStateSubject<TSubject>
    {
        IActionBuilder<TAction, TSubject> On<TAction>() where TAction : class, IAction;
    }
}

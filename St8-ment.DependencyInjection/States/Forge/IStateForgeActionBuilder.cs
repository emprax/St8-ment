using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    public interface IStateForgeActionBuilder<out TAction, out TSubject>
        where TSubject : StateSubject
        where TAction : class, IAction
    {
        void Handle<THandler>() where THandler : class, IActionHandler<TAction, TSubject>;
    }
}
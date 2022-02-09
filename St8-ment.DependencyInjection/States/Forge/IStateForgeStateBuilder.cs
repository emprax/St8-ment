using St8Ment.States;

namespace St8Ment.DependencyInjection.States.Forge
{
    public interface IStateForgeStateBuilder<out TSubject> where TSubject : StateSubject
    {
        IStateForgeActionBuilder<TAction, TSubject> On<TAction>() where TAction : class, IAction;
    }
}
using St8_ment.V1;

namespace St8_ment.DependencyInjection.V1
{
    public interface IActionConfiguration<TAction, TState> 
        where TAction : IAction
        where TState : class, IState
    {
        void Transition<TTransitioner>() where TTransitioner : class, IStateTransitioner<StateTransaction<TAction, TState>>;
    }
}

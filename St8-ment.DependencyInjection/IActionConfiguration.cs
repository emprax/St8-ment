namespace St8_ment.DependencyInjection
{
    public interface IActionConfiguration<TAction, TState> 
        where TAction : IAction
        where TState : class, IState
    {
        void Transition<TTransition>() where TTransition : class, IStateTransition<StateTransaction<TAction, TState>>;
    }
}

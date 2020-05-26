namespace St8_ment
{
    public interface IStateMachine<TContext> where TContext : IStateContext
    {
        TState Find<TState>() where TState : class, IState<TContext>;
    }
}
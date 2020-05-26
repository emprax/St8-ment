namespace St8_ment
{
    public interface IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        TState Find<TState>(TContext context) where TState : class, IState<TContext>;
    }
}
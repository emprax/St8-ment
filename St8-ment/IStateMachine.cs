namespace St8_ment
{
    public interface IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        bool Apply<TState>(TContext context) where TState : class, IState<TContext>;
    }
}
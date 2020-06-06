namespace St8_ment.V1
{
    public interface IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        bool Apply<TState>(TContext context) where TState : class, IState<TContext>;
    }
}
namespace St8_ment.V2
{
    public interface IStateMachine<TContext> where TContext : IStateContext<TContext>
    {
        IStateTransitionApplier<TState, TContext> For<TState>(TState state) where TState : IState<TContext>;
    }
}

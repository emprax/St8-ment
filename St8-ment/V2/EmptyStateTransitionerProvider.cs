namespace St8_ment.V2
{
    public class EmptyStateTransitionerProvider<TState, TContext> : IStateTransitionerProvider<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : class, IStateContext<TContext>
    {
        public IStateTransitioner<TAction, TState, TContext> Find<TAction>() where TAction : IAction => null;
    }
}

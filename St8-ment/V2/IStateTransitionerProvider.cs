namespace St8_ment.V2
{
    public interface IStateTransitionerProvider { }

    public interface IStateTransitionerProvider<TState, TContext> : IStateTransitionerProvider 
        where TContext : IStateContext<TContext>
        where TState : IState<TContext>
    {
        IStateTransitioner<TAction, TState, TContext> Find<TAction>() where TAction : IAction;
    }
}
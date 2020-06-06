namespace St8_ment.V2
{
    public interface IStateTransitionerProvider { }

    public interface IStateTransitionerProvider<TState, TContext> : IStateTransitionerProvider
        where TState : IState<TContext> 
        where TContext : IStateContext<TContext>
    {
        IStateTransitioner<TAction, TState, TContext> Find<TAction>() where TAction : IAction;
    }
}
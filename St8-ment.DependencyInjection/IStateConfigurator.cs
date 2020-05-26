namespace St8_ment.DependencyInjection
{
    public interface IStateConfigurator<TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        IActionConfiguration<TAction, TState> On<TAction>() where TAction : IAction<TState>;
    }
}
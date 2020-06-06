using St8_ment.V1;

namespace St8_ment.DependencyInjection.V1
{
    public interface IStateConfigurator<TState, TContext> 
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        IActionConfiguration<TAction, TState> On<TAction>() where TAction : IAction;
    }
}
using St8_ment.V2;

namespace St8_ment.DependencyInjection.V2
{
    public interface IStateConfigurator<TState, TContext>
        where TState : class, IState<TContext>
        where TContext : IStateContext<TContext>
    {
        IActionConfiguration<TAction, TState, TContext> On<TAction>() where TAction : IAction;
    }
}

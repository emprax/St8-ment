using St8_ment.V2;

namespace St8_ment.DependencyInjection.V2
{
    public interface IActionConfiguration<TAction, TState, TContext>
        where TAction : IAction
        where TState : class, IState<TContext>
        where TContext : class, IStateContext<TContext>
    {
        void Transition<TTransitioner>() where TTransitioner : class, IStateTransitioner<TAction, TState, TContext>;
    }
}

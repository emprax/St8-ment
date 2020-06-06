using System.Threading.Tasks;

namespace St8_ment.V2
{
    public class EmptyStateTransitionProvider<TState, TContext> : IStateTransitionerProvider<TState, TContext>
        where TState : IState<TContext>
        where TContext : IStateContext<TContext>
    {
        public IStateTransitioner<TAction, TState, TContext> Find<TAction>() where TAction : IAction => null;
    }
}

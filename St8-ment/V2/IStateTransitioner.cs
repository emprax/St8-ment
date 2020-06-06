using System.Threading.Tasks;

namespace St8_ment.V2
{
    public interface IStateTransitioner<TAction, TState, TContext> : IStateTransitionerMarker
        where TAction : IAction
        where TState : IState<TContext>
        where TContext : IStateContext<TContext>
    {
        Task<bool> Apply(IStateTransaction<TAction, TState> action);
    }
}
using System.Threading.Tasks;

namespace St8_ment.V2
{
    public interface IStateTransitionApplier<TState, TContext> 
        where TState : IState<TContext> 
        where TContext : IStateContext<TContext>
    {
        Task<bool> Apply<TAction>(TAction action) where TAction : IAction;
    }
}

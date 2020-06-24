using System.Threading.Tasks;

namespace St8_ment.V2
{
    public class ActionAccepter<TContext, TState> : IActionAccepter<TContext> 
        where TContext : IStateContext<TContext>
        where TState : IState<TContext>
    {
        private readonly IStateTransitionApplier<TState, TContext> stateTransitionApplier;

        public ActionAccepter(IStateTransitionApplier<TState, TContext> stateTransitionApplier)
        {
            this.stateTransitionApplier = stateTransitionApplier;
        }

        public Task<bool> Apply<TAction>(TAction action) where TAction : IAction
        {
            return this.stateTransitionApplier.Apply(action);
        }
    }
}

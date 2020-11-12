using System.Threading.Tasks;

namespace St8_ment.V2
{
    public class ActionAccepter<TContext, TState> : IActionAccepter<TContext> 
        where TContext : class, IStateContext<TContext>
        where TState : IState<TContext>
    {
        private readonly IStateTransitionerApplier<TState, TContext> stateTransitionApplier;

        public ActionAccepter(IStateTransitionerApplier<TState, TContext> stateTransitionApplier)
        {
            this.stateTransitionApplier = stateTransitionApplier;
        }

        public Task<bool> Apply<TAction>(TAction action) where TAction : IAction
        {
            return this.stateTransitionApplier.Apply(action);
        }
    }
}

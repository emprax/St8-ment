using System.Threading;
using System.Threading.Tasks;

namespace St8_ment
{
    public abstract class StateTransition<TState, TContext, TAction> : IStateTransition<StateTransaction<TAction, TState>> 
        where TAction : IAction<TState> 
        where TState : class, IState<TContext>
        where TContext : IStateContext
    {
        private readonly IStateMachine<TContext> stateMachine;

        protected StateTransition(IStateMachine<TContext> stateMachine) => this.stateMachine = stateMachine;

        protected abstract Task Transition(StateTransaction<TAction, TState> action, IStateMachine<TContext> stateMachine, CancellationToken cancellationToken);

        public Task Handle(StateTransaction<TAction, TState> transaction, CancellationToken cancellationToken)
        {
            return this.Transition(transaction, this.stateMachine, cancellationToken);
        }
    }
}
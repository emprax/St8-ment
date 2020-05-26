using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class StartTransition : StateTransition<InitialState, ExampleContext, StartAction>
    {
        public StartTransition(IStateMachine<ExampleContext> stateMachine) : base(stateMachine) { }

        protected override Task Transition(StateTransaction<StartAction, InitialState> transaction, IStateMachine<ExampleContext> stateMachine, CancellationToken cancellationToken)
        {
            transaction.State.Context.State = stateMachine.Find<ProcessingState>();
            return Task.CompletedTask;
        }
    }
}

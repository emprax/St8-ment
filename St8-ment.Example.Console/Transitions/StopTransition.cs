using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class StopTransition : StateTransition<ProcessingState, ExampleContext, StopAction>
    {
        public StopTransition(IStateMachine<ExampleContext> stateMachine) : base(stateMachine) { }

        protected override Task Transition(StateTransaction<StopAction, ProcessingState> transaction, IStateMachine<ExampleContext> stateMachine, CancellationToken cancellationToken)
        {
            transaction.State.Context.SetState(stateMachine.Find<DoneState>(transaction.State.Context));
            return Task.CompletedTask;
        }
    }
}

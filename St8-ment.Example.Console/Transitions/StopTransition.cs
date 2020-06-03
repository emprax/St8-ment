using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class StopTransitioner : StateTransitioner<ProcessingState, ExampleContext, StopAction>
    {
        public StopTransitioner(IStateMachine<ExampleContext> stateMachine) : base(stateMachine) { }

        protected override Task Transition(StateTransaction<StopAction, ProcessingState> transaction, IStateMachine<ExampleContext> stateMachine, CancellationToken cancellationToken)
        {
            stateMachine.Apply<DoneState>(transaction.State.Context);
            return Task.CompletedTask;
        }
    }
}

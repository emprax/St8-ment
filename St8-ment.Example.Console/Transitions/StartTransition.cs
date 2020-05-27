using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class StartTransition : StateTransition<InitialState, ExampleContext, StartAction>
    {
        public StartTransition(IStateMachine<ExampleContext> stateMachine) : base(stateMachine) { }

        protected override Task Transition(StateTransaction<StartAction, InitialState> transaction, IStateMachine<ExampleContext> stateMachine, CancellationToken cancellationToken)
        {
            stateMachine.Apply<ProcessingState>(transaction.State.Context);
            return Task.CompletedTask;
        }
    }
}

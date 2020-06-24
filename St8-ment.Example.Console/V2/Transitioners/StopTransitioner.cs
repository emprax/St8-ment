using St8_ment.V2;
using System.Threading.Tasks;

namespace St8_ment.Example.Console.V2
{
    public class StopTransitioner : IStateTransitioner<StopAction, ProcessingState, ExampleContext>
    {
        public Task<bool> Apply(IStateTransaction<StopAction, ProcessingState> action)
        {
            System.Console.WriteLine("Stop-action came in with state: {0}", action.State.Name);
            action.State.Context.SetState(new DoneState(action.State.Context));
            return Task.FromResult(true);
        }
    }
}

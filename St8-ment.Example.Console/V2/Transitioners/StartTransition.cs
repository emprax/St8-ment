using St8_ment.V2;
using System.Threading.Tasks;

namespace St8_ment.Example.Console.V2
{
    public class StartTransitioner : IStateTransitioner<StartAction, InitialState, ExampleContext>
    {
        public Task<bool> Apply(IStateTransaction<StartAction, InitialState> action)
        {
            System.Console.WriteLine("Start-action came in with state: {0}", action.State.Name);
            action.State.Context.SetState(new ProcessingState(action.State.Context));
            return Task.FromResult(true);
        }
    }
}

using System.Threading.Tasks;
using St8_ment.StateMachines;

namespace St8_ment.Example.Console.StateMachines.Utilities
{
    public class ExampleCallback : ITransitionCallback<string>
    {
        public Task Execute(string action)
        {
            System.Console.WriteLine("   + Entered the example-callback with action: {0}.", action);
            return Task.CompletedTask;
        }
    }
}

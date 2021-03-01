using System.Threading.Tasks;
using St8_ment.StateMachines;

namespace St8_ment.Example.Console.StateMachines.Utilities
{
    public class FailureCallback : ITransitionCallback<object>
    {
        public Task Execute(object action)
        {
            System.Console.WriteLine("   + A failure occurred for action: {0}.", action);
            return Task.CompletedTask;
        }
    }
}

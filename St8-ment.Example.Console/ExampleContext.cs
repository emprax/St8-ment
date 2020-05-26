using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console
{
    public class ExampleContext : IStateContext
    {
        public ExampleContext(IState<ExampleContext> initial) => this.State = initial;

        public IState<ExampleContext> State { get; set; }

        public Task<bool> Apply<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction
        {
            return State.Accept(action, cancellationToken);
        }
    }
}

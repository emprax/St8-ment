using St8_ment.V1;
using System.Threading;
using System.Threading.Tasks;

namespace St8_ment.Example.Console.V1
{
    public class ExampleContext : IStateContext<ExampleContext>
    {
        public IState<ExampleContext> State { get; private set; }

        public Task<bool> Accept<TAction>(TAction action, CancellationToken cancellationToken) where TAction : IAction
        {
            return State.Accept(action, cancellationToken);
        }

        public void SetState<TState>(TState state) where TState : class, IState<ExampleContext>
        {
            this.State = state;
        }
    }
}

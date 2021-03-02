using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Example.Console.States.Utilities
{
    public class ExampleContext : IStateContext<ExampleContext>
    {
        private IState<ExampleContext> state;

        public void SetState(IState<ExampleContext> state) => this.state = state;

        public Task ApplyAnAction<TAction>(TAction action) where TAction : class, IAction
        {
            return state.Apply(action);
        }
    }
}

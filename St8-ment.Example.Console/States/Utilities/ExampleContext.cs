using System.Threading.Tasks;
using St8_ment.States;

namespace St8_ment.Example.Console.States.Utilities
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

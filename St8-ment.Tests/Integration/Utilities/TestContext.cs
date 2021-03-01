using System.Threading.Tasks;
using St8_ment.States;

namespace St8_ment.Tests.Integration.Utilities
{
    public class TestContext : IStateContext<TestContext>
    {
        public IState<TestContext> State { get; private set; }

        public TestContext() { }

        public TestContext(IState<TestContext> state) => State = state;

        public void SetState(IState<TestContext> state) => State = state;

        public Task<StateResponse> ApplyAction<TAction>(TAction action) where TAction : class, IAction
            => State.Apply(action);
    }
}

using System.Threading.Tasks;
using St8Ment.States;

namespace St8Ment.Tests.Integration.Utilities
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

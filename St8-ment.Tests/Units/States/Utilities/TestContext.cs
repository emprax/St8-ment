using St8_ment.States;

namespace St8_ment.Tests.Units.States
{
    public class TestContext : IStateContext<TestContext>
    {
        public IState<TestContext> State { get; private set; }

        public TestContext() { }

        public TestContext(IState<TestContext> state) => this.State = state;

        public void SetState(IState<TestContext> state) => this.State = state;
    }
}

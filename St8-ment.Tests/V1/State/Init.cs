using Moq;
using St8_ment.V1;

namespace St8_ment.Tests.V1.State
{
    public partial class StateTests
    {
        private readonly FakeContext context;
        private readonly IStateTransitionerProvider stateTransitionerProvider;
        private readonly IState<FakeContext> state;

        public StateTests()
        {
            this.context = new FakeContext();
            this.stateTransitionerProvider = Mock.Of<IStateTransitionerProvider>(MockBehavior.Strict);
            this.state = new TestState(this.context, this.stateTransitionerProvider);
        }
    }
}

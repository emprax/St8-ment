using St8_ment.V1;

namespace St8_ment.Tests.V1
{
    public class TestState : State<TestState, FakeContext>
    {
        public TestState(FakeContext context, IStateTransitionerProvider provider) : base(context, provider) { }

        protected override TestState GetSelf() => this;
    }
}

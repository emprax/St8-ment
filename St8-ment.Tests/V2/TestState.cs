using St8_ment.V2;

namespace St8_ment.Tests.V2
{
    public class TestState : State<TestState, FakeContext>
    {
        public TestState(FakeContext context) : base(context) { }
    }
}

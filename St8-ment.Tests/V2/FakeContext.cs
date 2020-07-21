using St8_ment.V2;

namespace St8_ment.Tests.V2
{
    public class FakeContext : IStateContext<FakeContext>
    {
        public IState<FakeContext> State { get; private set; }

        public void SetState<TState>(TState state) where TState : class, IState<FakeContext>
        {
            this.State = state;
        }
    }
}

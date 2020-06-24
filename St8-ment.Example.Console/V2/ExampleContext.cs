using St8_ment.V2;

namespace St8_ment.Example.Console.V2
{
    public class ExampleContext : IStateContext<ExampleContext>
    {
        public IState<ExampleContext> State { get; private set; }

        public void SetState<TState>(TState state) where TState : class, IState<ExampleContext>
        {
            this.State = state;
        }
    }
}

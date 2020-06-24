using St8_ment.V2;

namespace St8_ment.Example.Console.V2
{
    public class InitialState : State<InitialState, ExampleContext>
    {
        public InitialState(ExampleContext context) : base(context) => this.Name = "The initial state";

        public string Name { get; }
    }
}

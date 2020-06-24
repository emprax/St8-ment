using St8_ment.V2;

namespace St8_ment.Example.Console.V2
{
    public class DoneState : State<DoneState, ExampleContext>
    {
        public DoneState(ExampleContext context) : base(context) => this.Name = "The final state";

        public string Name { get; }
    }
}

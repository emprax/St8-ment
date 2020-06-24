using St8_ment.V2;

namespace St8_ment.Example.Console.V2
{
    public class ProcessingState : State<ProcessingState, ExampleContext>
    {
        public ProcessingState(ExampleContext context) : base(context) => this.Name = "The processing state";

        public string Name { get; }
    }
}

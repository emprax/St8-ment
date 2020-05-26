namespace St8_ment.Example.Console
{
    public class ProcessingState : State<ProcessingState, ExampleContext>
    {
        public ProcessingState(ExampleContext context, IStateTransitionProvider provider) : base(context, provider)
        {
            this.Name = "The processing state";
        }

        public string Name { get; }

        protected override ProcessingState GetSelf() => this;
    }
}

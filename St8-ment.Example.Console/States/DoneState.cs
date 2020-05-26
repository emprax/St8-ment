namespace St8_ment.Example.Console
{
    public class DoneState : State<DoneState, ExampleContext>
    {
        public DoneState(ExampleContext context, IStateTransitionProvider provider) : base(context, provider)
        {
            this.Name = "The processing state";
        }

        public string Name { get; }

        protected override DoneState GetSelf() => this;
    }
}

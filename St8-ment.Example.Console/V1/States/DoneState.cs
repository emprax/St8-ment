using St8_ment.V1;

namespace St8_ment.Example.Console.V1
{
    public class DoneState : State<DoneState, ExampleContext>
    {
        public DoneState(ExampleContext context, IStateTransitionerProvider provider) : base(context, provider)
        {
            this.Name = "The processing state";
        }

        public string Name { get; }

        protected override DoneState GetSelf() => this;
    }
}

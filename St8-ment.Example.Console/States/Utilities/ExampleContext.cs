using St8Ment.States;

namespace St8Ment.Example.Console.States.Utilities
{
    public class ExampleContext : ExtendedStateSubject<ExampleContext>
    {
        public ExampleContext() { }

        public ExampleContext(ExampleState stateId) => base.StateId = stateId;
    }
}

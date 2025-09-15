using St8Ment.States.Abstractions.Core;
using St8Ment.States.Core;

namespace St8Ment.Example.Console.States.Utilities;

public class ExampleContext(StateId id) : IStateSubject<ExampleContext>
{
    public State State { get; } = new(id);
}

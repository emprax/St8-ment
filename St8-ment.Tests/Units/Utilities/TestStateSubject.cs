using St8Ment.States.Abstractions.Core;
using St8Ment.States.Core;

namespace St8Ment.Tests.Units.Utilities;

public class TestStateSubject : IStateSubject<TestStateSubject>
{
    public TestStateSubject(StateId id) => this.State = new(id);

    public State State { get; }
}
using St8Ment.States.Abstractions.Core;
using St8Ment.States.Core;

namespace St8Ment.Tests.Integration.Utilities;

public class TesTSubject(StateId id) : IStateSubject<TesTSubject>
{
    public State State { get; } = new(id);
}

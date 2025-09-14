using St8Ment.States.Abstractions.Core;

namespace St8Ment.Tests.Units.Utilities;

public record TestAction(string ActionName) : IAction<TestStateSubject>;
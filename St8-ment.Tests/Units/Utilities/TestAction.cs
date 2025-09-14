using St8Ment.States.Abstractions.Core;

namespace St8Ment.Tests.Units.Utilities;

public record TestAction(string ActionName) : IAction<TestStateSubject>;

public class OpenAction : IAction<TestStateSubject>;

public class CloseAction : IAction<TestStateSubject>;
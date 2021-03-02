using St8Ment.States;

namespace St8Ment.Tests.Units.Utilities
{
    public class TestAction : IAction
    {
        public TestAction(string actionName) => ActionName = actionName;

        public string ActionName { get; }
    }
}

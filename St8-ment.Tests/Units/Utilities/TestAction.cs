using St8_ment.States;

namespace St8_ment.Tests.Units.Utilities
{
    public class TestAction : IAction
    {
        public TestAction(string actionName) => ActionName = actionName;

        public string ActionName { get; }
    }
}

using St8_ment.States;

namespace St8_ment.Tests.Units.States
{
    public class TestAction : IAction
    {
        public TestAction(string actionName) => this.ActionName = actionName;

        public string ActionName { get; }
    }
}

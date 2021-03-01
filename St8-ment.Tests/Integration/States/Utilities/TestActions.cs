using St8_ment.States;

namespace St8_ment.Tests.Integration.States
{
    public class Test1Action : IAction
    {
        public Test1Action() => this.ActionName = "TEST-1";

        public string ActionName { get; }
    }

    public class Test2Action : IAction
    {
        public Test2Action() => this.ActionName = "TEST-2";

        public string ActionName { get; }
    }

    public class Test3Action : IAction
    {
        public Test3Action() => this.ActionName = "TEST-3";

        public string ActionName { get; }
    }
}

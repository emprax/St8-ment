using St8_ment.States;

namespace St8_ment.Tests.Integration.Utilities
{
    public class Test1Action : IAction
    {
        public Test1Action() => ActionName = "TEST-1";

        public string ActionName { get; }
    }

    public class Test2Action : IAction
    {
        public Test2Action() => ActionName = "TEST-2";

        public string ActionName { get; set; }
    }

    public class Test3Action : IAction
    {
        public Test3Action() => ActionName = "TEST-3";

        public string ActionName { get; }
    }
}

using St8Ment.States.Abstractions.Core;

namespace St8Ment.Tests.Integration.Utilities;

public class Test1Action : IAction<TesTSubject>
{
    public Test1Action() => this.ActionName = "TEST-1";

    public string ActionName { get; }
}

public class Test2Action : IAction<TesTSubject>
{
    public Test2Action() => this.ActionName = "TEST-2";

    public string ActionName { get; set; }
}

public class Test3Action : IAction<TesTSubject>
{
    public Test3Action() => this.ActionName = "TEST-3";

    public string ActionName { get; }
}

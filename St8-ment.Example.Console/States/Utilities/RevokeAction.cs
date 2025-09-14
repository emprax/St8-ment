using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.Example.Console.States.Utilities;

public class RevokeAction : IAction<ExampleContext>
{
    public RevokeAction(string reason)
    {
        this.Reason = reason;
        this.At = DateTime.UtcNow;
    }

    public string Reason { get; }

    public DateTime At { get; set; }
}

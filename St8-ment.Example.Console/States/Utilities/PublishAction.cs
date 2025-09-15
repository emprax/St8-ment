using St8Ment.States.Abstractions.Core;
using System;

namespace St8Ment.Example.Console.States.Utilities;

public class PublishAction : IAction<ExampleContext>
{
    public PublishAction() => this.At = DateTime.UtcNow;

    public DateTime At { get; }
}

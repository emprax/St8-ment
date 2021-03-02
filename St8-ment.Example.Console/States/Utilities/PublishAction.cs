using System;
using St8Ment.States;

namespace St8Ment.Example.Console.States.Utilities
{
    public class PublishAction : IAction
    {
        public PublishAction() => this.At = DateTime.UtcNow;

        public DateTime At { get; }
    }
}

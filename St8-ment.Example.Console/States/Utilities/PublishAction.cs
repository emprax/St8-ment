using System;
using St8_ment.States;

namespace St8_ment.Example.Console.States.Utilities
{
    public class PublishAction : IAction
    {
        public PublishAction() => this.At = DateTime.UtcNow;

        public DateTime At { get; }
    }
}

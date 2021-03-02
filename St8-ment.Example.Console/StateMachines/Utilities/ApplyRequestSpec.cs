using System;
using System.Linq.Expressions;
using SpeciFire;

namespace St8Ment.Example.Console.StateMachines.Utilities
{
    public class ApplyRequestSpec : Spec<string>
    {
        public override Expression<Func<string, bool>> AsExpression() => text => !string.IsNullOrWhiteSpace(text);
    }
}

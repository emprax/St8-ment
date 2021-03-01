using System;
using System.Linq.Expressions;
using SpeciFire;

namespace St8_ment.Example.Console.StateMachines.Utilities
{
    public class ApplyRequestSpec : Spec<string>
    {
        public override Expression<Func<string, bool>> AsExpression() => text => !string.IsNullOrWhiteSpace(text);
    }
}

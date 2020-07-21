using St8_ment.V1;
using System;
using System.Collections.Generic;

namespace St8_ment.Tests.V1.StateMachine
{
    public partial class StateMachineTestsV1
    {
        private readonly IDictionary<int, Func<FakeContext, IState<FakeContext>>> registrations;
        private readonly IStateMachine<FakeContext> stateMachine;

        public StateMachineTestsV1()
        {
            this.registrations = new Dictionary<int, Func<FakeContext, IState<FakeContext>>>();
            this.stateMachine = new StateMachine<FakeContext>(this.registrations);
        }
    }
}

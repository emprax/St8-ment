using St8_ment.V2;
using System.Collections.Generic;

namespace St8_ment.Tests.V2.StateMachine
{
    public partial class StateMachineTests
    {
        private readonly IDictionary<int, IStateTransitionerProvider> registrations;
        private readonly IStateMachine<FakeContext> stateMachine;

        public StateMachineTests()
        {
            this.registrations = new Dictionary<int, IStateTransitionerProvider>();
            this.stateMachine = new StateMachine<FakeContext>(this.registrations);
        }
    }
}

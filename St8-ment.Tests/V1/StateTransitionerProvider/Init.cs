using St8_ment.V1;
using System;
using System.Collections.Generic;

namespace St8_ment.Tests.V1.StateTransitionerProvider
{
    public partial class StateTransitionerProviderTests
    {
        private readonly IDictionary<int, Func<IStateTransitionerMarker>> transitionerRegistrations;
        private readonly IStateTransitionerProvider provider;

        public StateTransitionerProviderTests()
        {
            this.transitionerRegistrations = new Dictionary<int, Func<IStateTransitionerMarker>>();
            this.provider = new StateTransitionerProvider<TestState, FakeContext>(this.transitionerRegistrations);
        }
    }
}

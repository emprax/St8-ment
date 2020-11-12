using St8_ment.V1;
using System;
using System.Collections.Generic;

namespace St8_ment.Tests.V1.StateTransitionerProvider
{
    public partial class StateReducerProviderTests
    {
        private readonly IDictionary<int, Func<IStateTransitionerMarker>> transitionerRegistrations;
        private readonly IStateTransitionerProvider provider;

        public StateReducerProviderTests()
        {
            this.transitionerRegistrations = new Dictionary<int, Func<IStateTransitionerMarker>>();
            this.provider = new StateTransitionerProvider<TestState, FakeContext>(this.transitionerRegistrations);
        }
    }
}

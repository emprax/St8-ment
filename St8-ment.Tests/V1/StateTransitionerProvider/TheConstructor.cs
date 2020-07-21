using St8_ment.V1;
using System;
using Xunit;

namespace St8_ment.Tests.V1.StateTransitionerProvider
{
    public partial class StateTransitionerProviderTests
    {
        public class TheConstructor : StateTransitionerProviderTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenProviderIsNull()
                => Assert.Throws<ArgumentNullException>(() => new StateTransitionerProvider<TestState, FakeContext>(null));

            [Fact]
            public void ShouldConstruct()
                => Assert.NotNull(new StateTransitionerProvider<TestState, FakeContext>(this.transitionerRegistrations));
        }
    }
}

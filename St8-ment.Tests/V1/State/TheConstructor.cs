using System;
using Xunit;

namespace St8_ment.Tests.V1.State
{
    public partial class StateTests
    {
        public class TheConstructor : StateTests
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenContextIsNull()
                => Assert.Throws<ArgumentNullException>(() => new TestState(null, this.stateTransitionerProvider));

            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenStateTransitionerProviderIsNull()
                => Assert.Throws<ArgumentNullException>(() => new TestState(this.context, null));

            [Fact]
            public void ShouldConstruct()
                => Assert.NotNull(new TestState(this.context, this.stateTransitionerProvider));
        }
    }
}

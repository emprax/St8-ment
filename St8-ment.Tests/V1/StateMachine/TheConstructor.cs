using St8_ment.V1;
using System;
using Xunit;

namespace St8_ment.Tests.V1.StateMachine
{
    public partial class StateMachineTestsV1
    {
        public class TheConstructor : StateMachineTestsV1
        {
            [Fact]
            public void ShouldThrowArgumentNullExceptionWhenRegistrationAreNull()
                => Assert.Throws<ArgumentNullException>(() => new StateMachine<FakeContext>(null));

            [Fact]
            public void ShouldConstruct()
                => Assert.NotNull(new StateMachine<FakeContext>(this.registrations));
        }
    }
}

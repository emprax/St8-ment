using St8_ment.V2;
using System;
using Xunit;

namespace St8_ment.Tests.V2.StateMachine
{
    public partial class StateMachineTests
    {
        public class TheConstructor : StateMachineTests
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

using Moq;
using St8_ment.V1;
using Xunit;

namespace St8_ment.Tests.V1.StateTransitionerProvider
{
    public partial class StateTransitionerProviderTests
    {
        public class TheFindMethod : StateTransitionerProviderTests
        {
            [Fact]
            public void ShouldReturnNullWhenValueCannotBeFoundForKey()
            {
                // Act & Assert
                Assert.Null(this.provider.Find<StateTransaction<TestAction, TestState>>());
            }

            [Fact]
            public void ShouldReturnNullWhenResultingProviderIsNull()
            {
                // Arrange
                this.transitionerRegistrations.Add(typeof(StateTransaction<TestAction, TestState>).GetHashCode(), null);

                // Act & Assert
                Assert.Null(this.provider.Find<StateTransaction<TestAction, TestState>>());
            }

            [Fact]
            public void ShouldReturnNullWhenResultingTransitionerIsNull()
            {
                // Arrange
                this.transitionerRegistrations.Add(typeof(StateTransaction<TestAction, TestState>).GetHashCode(), () => null);

                // Act & Assert
                Assert.Null(this.provider.Find<StateTransaction<TestAction, TestState>>());
            }

            [Fact]
            public void ShouldReturnTransitioner()
            {
                // Arrange
                var transitioner = Mock.Of<IStateTransitioner<StateTransaction<TestAction, TestState>>>();
                this.transitionerRegistrations.Add(typeof(StateTransaction<TestAction, TestState>).GetHashCode(), () => transitioner);

                // Act & Assert
                Assert.NotNull(this.provider.Find<StateTransaction<TestAction, TestState>>());
            }
        }
    }
}

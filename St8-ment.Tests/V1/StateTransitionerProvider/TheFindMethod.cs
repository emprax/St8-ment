using Moq;
using St8_ment.V1;
using Xunit;

namespace St8_ment.Tests.V1.StateTransitionerProvider
{
    public partial class StateReducerProviderTests
    {
        public class TheFindMethod : StateReducerProviderTests
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
            public void ShouldReturnNullWhenResultingReducerIsNull()
            {
                // Arrange
                this.transitionerRegistrations.Add(typeof(StateTransaction<TestAction, TestState>).GetHashCode(), () => null);

                // Act & Assert
                Assert.Null(this.provider.Find<StateTransaction<TestAction, TestState>>());
            }

            [Fact]
            public void ShouldReturnReducer()
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
